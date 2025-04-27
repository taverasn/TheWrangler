using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance;
    public UIEvents UIEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Game Events Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        UIEvents = new UIEvents();
        Instance = this;
    }
}

public class UIEvents
{
    public event Action onOpenPlayerInventory;
    public void OpenPlayerInventory() => onOpenPlayerInventory?.Invoke();
    
    public event Action<Item, bool, bool> onMoveItemToSeparateInventory;
    public void MoveItemToSeparateInventory(Item item, bool toPlayer, bool swapItemsBothWays) => onMoveItemToSeparateInventory?.Invoke(item, toPlayer, swapItemsBothWays);
    
    public event Action<bool> onRequestItemFromSeparateInventory;
    public void RequestItemFromSeparateInventory(bool swapItemsBothWays) => onRequestItemFromSeparateInventory?.Invoke(swapItemsBothWays);
}