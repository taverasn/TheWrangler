using System.Collections.Generic;
using JUTPS.CameraSystems;
using NUnit.Framework;
using UnityEngine;

public class CraftingTableUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory PlayerInventory;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject recipeSlotPrefab;
    [SerializeField] private CraftingTable craftingTable;
    [SerializeField] private Transform recipeSlotParent;
    [SerializeField] private float yOffset = 270;
    List<RecipeSlot> recipeSlots = new List<RecipeSlot>();

    private void Start()
    {
        foreach (RecipeSO recipe in craftingTable.recipes)
        {
            RecipeSlot recipeSlot = Instantiate(recipeSlotPrefab).GetComponent<RecipeSlot>();
            recipeSlot.transform.SetParent(recipeSlotParent);
            recipeSlot.SetRecipe(recipe);
            recipeSlots.Add(recipeSlot);
        }

        recipeSlotParent.GetComponent<RectTransform>().sizeDelta = new Vector2(recipeSlotParent.GetComponent<RectTransform>().sizeDelta.x, (recipeSlots.Count + 1 )/ 2) * yOffset;
    }

    private void OnEnable()
    {
        inventory.onInventoryUpdated += UpdateCraftingTable;
    }

    private void OnDisable()
    {
        inventory.onInventoryUpdated -= UpdateCraftingTable;
    }

    public void EnableUI()
    {
        UpdateCraftingTable();
    }

    private void UpdateCraftingTable()
    {
        foreach (RecipeSlot recipeSlot in recipeSlots)
        {
            recipeSlot.UpdateRecipe(PlayerInventory.CanCraft(recipeSlot.recipeSO), inventory.CanAddItem(recipeSlot.recipeSO.item));
        }
    }

    public void CraftItem(RecipeSO recipe)
    {
        Item item = craftingTable.CraftItem(PlayerInventory, recipe);
        inventory.AddItem(item);
        UpdateCraftingTable();
    }
}