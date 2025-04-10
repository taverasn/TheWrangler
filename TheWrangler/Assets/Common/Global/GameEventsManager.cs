using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance;

    public AIEvents AIEvents { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        AIEvents = new AIEvents();
    }
}
