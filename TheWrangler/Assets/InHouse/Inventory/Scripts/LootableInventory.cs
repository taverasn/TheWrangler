using System;
using System.Collections.Generic;
using JUTPS.CameraSystems;
using JUTPS.CharacterBrain;
using JUTPS.JUInputSystem;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class LootableInventory : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryUI InventoryUI;
    [SerializeField] private CraftingTableUI craftingTableUI;
    [SerializeField] private TextMeshProUGUI popupText;
    [ShowInInspector] public ItemAmountDictionary items;
    private JUTPSInputControlls _inputs;
    private JUCharacterBrain JUCharacter;

    private void Start()
    {
        foreach (KeyValuePair<ItemSO, int> itemPair in items)
        {
            inventory.AddItem(new Item(itemPair.Value, itemPair.Key));
        }
    }

    private void OnEnable()
    {
        _inputs = JUInput.Instance().InputActions;
        _inputs.Player.Interact.started += OnOpenInventory;
    }

    private void OnDisable()
    {
        _inputs.Player.Interact.started -= OnOpenInventory;
    }

    public void OnOpenInventory(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (JUCharacter == null) return;
        GameEventsManager.Instance.UIEvents.OpenPlayerInventory();
        InventoryUI.EnableUI();
        craftingTableUI.EnableUI();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            JUCharacter = other.gameObject.GetComponent<JUCharacterBrain>();
            popupText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            JUCharacter = null;
            popupText.gameObject.SetActive(false);
        }
    }
}
