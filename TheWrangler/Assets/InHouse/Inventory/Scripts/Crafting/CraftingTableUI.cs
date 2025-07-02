using System.Collections.Generic;
using JUTPS.CameraSystems;
using NUnit.Framework;
using UnityEngine;

public class CraftingTableUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory PlayerInventory;
    [SerializeField] private GameObject recipeSlotPrefab;
    [SerializeField] private Transform recipeSlotParent;
    [SerializeField] private float yOffset = 270;
    private Inventory inventory;
    private CraftingTable craftingTable;
    List<RecipeSlot> recipeSlots = new List<RecipeSlot>();
    private bool active;

    private void Start()
    {

    }

    private void OnEnable()
    {
        GameEventsManager.Instance.UIEvents.onToggleCraftingUI += ToggleUI;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.UIEvents.onToggleCraftingUI -= ToggleUI;
    }

    public void ToggleUI(CraftingTable _craftingTable, Inventory _inventory)
    {
        inventory = _inventory;
        craftingTable = _craftingTable;

        active = !active;

        if (active)
        {
            inventory.onInventoryUpdated += UpdateCraftingTable;
            foreach (RecipeSO recipe in craftingTable.recipes)
            {
                RecipeSlot recipeSlot = Instantiate(recipeSlotPrefab).GetComponent<RecipeSlot>();
                recipeSlot.transform.SetParent(recipeSlotParent);
                recipeSlot.SetRecipe(recipe);
                recipeSlots.Add(recipeSlot);
            }

            recipeSlotParent.GetComponent<RectTransform>().sizeDelta = new Vector2(recipeSlotParent.GetComponent<RectTransform>().sizeDelta.x, (recipeSlots.Count + 1) / 2) * yOffset;
            UpdateCraftingTable();
        }
        else
        {
            foreach (Transform child in recipeSlotParent.transform) Destroy(child.gameObject);
            recipeSlots.Clear();
            inventory.onInventoryUpdated -= UpdateCraftingTable;
        }

    }

    private void UpdateCraftingTable()
    {
        foreach (RecipeSlot recipeSlot in recipeSlots)
        {
            (bool canCraft, List<Item> ingredients) = PlayerInventory.CanCraft(recipeSlot.recipeSO);
            recipeSlot.UpdateRecipe(canCraft, ingredients, inventory.CanAddItem(recipeSlot.recipeSO.item));
        }
    }

    public void CraftItem(RecipeSO recipe)
    {
        Item item = craftingTable.CraftItem(PlayerInventory, recipe);
        inventory.AddItem(item);
        UpdateCraftingTable();
    }
}