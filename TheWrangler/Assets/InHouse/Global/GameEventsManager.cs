using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Game Events Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }
}
