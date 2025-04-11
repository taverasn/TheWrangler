using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance;

    public AIEvents AIEvents { get; private set; }
    public WeaponInputEvents weaponInputEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Game Events Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }

        // initialize all events
        weaponInputEvents = new WeaponInputEvents();
        AIEvents = new AIEvents();

        Instance = this;
    }
}
