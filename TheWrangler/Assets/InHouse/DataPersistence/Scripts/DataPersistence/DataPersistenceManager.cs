using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Auto Saving Configuration")]
    [SerializeField] private float autoSaveTimeSeconds = 60f;

    private GameData GameData = new GameData();
    private List<IDataPersistence> DataPersistenceObjects;
    private FileDataHandler FileDataHandler;

    private string selectedProfileId = "";

    private Coroutine autoSaveCoroutine;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persisence Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        this.FileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.DataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();

        // start up the auto saving coroutine
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            autoSaveCoroutine = StartCoroutine(AutoSave());
        }
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        // update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
        // load the game, which will use that profile, updating our game data accordingly

        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        // delete the data for this profile id
        FileDataHandler.Delete(profileId);
        // initialize the selected profile id
        InitializeSelectedProfileId();
        // reload the game so that our data matches the newly selected profile id
        LoadGame();
    }

    public void DeleteAllProfiles()
    {
        FileDataHandler.DeleteAllProfiles();
        this.GameData = null;
    }

    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = FileDataHandler.GetMostRecentlyUpdatedProfileId();

        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }

    public void NewGame()
    {
        this.GameData = new GameData();
    }

    private void LoadGame()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence)
        {
            return;
        }

        // Load any saved data from a file useing the data handler
        this.GameData = FileDataHandler.Load(selectedProfileId);

        // start a new game if the data is null and we're configured to intialize data for debugging purposes
        if (this.GameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        // If the version number is out of date start a new game
        if (this.GameData != null)
        {
            if (this.GameData.version != (new GameData()).version)
            {
                DeleteAllProfiles();
                Debug.LogWarning("Game Version out of date creating new save");
                return;
            }
        }

        // if no data can be loaded, don't continue
        if (this.GameData == null)
        {
            Debug.Log("No data was found. A new game needs to be started before data can be loaded.");
            return;
        }


        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObject in this.DataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(GameData);
        }
    }

    public void SaveGame()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence)
        {
            return;
        }

        // if we don't have any data to save, log a warning here
        if (this.GameData == null)
        {
            Debug.LogWarning("No data was found. A new game needs to be started before data can be saved");
            return;
        }
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObject in this.DataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(GameData);
        }

        // timestamp the data so we know when it was last saved
        GameData.lastUpdated = System.DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        FileDataHandler.Save(GameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            SaveGame();
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        // Find all objects of Type IDataPersistence in the current scene
        // FindObjectsOfType takes in an optional boolean that will include inactive gameobjects
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return GameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return FileDataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            SaveGame();
        }
    }
}
