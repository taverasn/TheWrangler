using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance;
    public UIEvents UIEvents;
    public NeedsEvents NeedsEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Game Events Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        UIEvents = new UIEvents();
        NeedsEvents = new NeedsEvents();
        Instance = this;
    }
}

public class UIEvents
{
    public event Action onOpenPlayerInventory;
    public void OpenPlayerInventory() => onOpenPlayerInventory?.Invoke();
    
    public event Action<Item, bool, bool> onMoveItemToSeparateInventory;
    public void MoveItemToSeparateInventory(Item item, bool toPlayer, bool swapItemsBothWays) => onMoveItemToSeparateInventory?.Invoke(item, toPlayer, swapItemsBothWays);
    
    public event Action<bool, InventorySlot> onRequestItemFromSeparateInventory;
    public void RequestItemFromSeparateInventory(bool swapItemsBothWays, InventorySlot swapCheckSlot) => onRequestItemFromSeparateInventory?.Invoke(swapItemsBothWays, swapCheckSlot);
    
    public event Action onCancelRequestItemFromSeparateInventory;
    public void CancelRequestItemFromSeparateInventory() => onCancelRequestItemFromSeparateInventory?.Invoke();

    public event Action<CraftingTable, Inventory> onToggleCraftingUI;
    public void ToggleCraftingUI(CraftingTable craftingTable, Inventory inventory) => onToggleCraftingUI?.Invoke(craftingTable, inventory);
    
    public event Action<Inventory> onToggleInventoryUI;
    public void ToggleInventoryUI(Inventory inventory) => onToggleInventoryUI?.Invoke(inventory);
}

public class NeedsEvents
{
    public Action<NeedsUpdateEvent> onUpdateNeeds;
    public void UpdateNeeds(NeedsUpdateEvent needsUpdateEvent) => onUpdateNeeds?.Invoke(needsUpdateEvent);
    
    public Action<NeedsBroadcastEvent> onBroadcastNeedsUpdate;
    public void BroadcastNeedsUpdate(NeedsBroadcastEvent needsBroadcastEvent) => onBroadcastNeedsUpdate?.Invoke(needsBroadcastEvent);
}