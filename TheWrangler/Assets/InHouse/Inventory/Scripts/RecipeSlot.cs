using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    public RecipeSO recipeSO { get; private set; }
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject ingredientSlotPrefab;
    [SerializeField] private GameObject ingredientSlotParent;
    [SerializeField] private Button button;
    List<IngredientSlot> ingredientSlots = new List<IngredientSlot>();
    private CraftingTableUI craftingTableUI;

    private void Start()
    {
        craftingTableUI = GetComponentInParent<CraftingTableUI>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(CraftItem);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(CraftItem);
    }

    public void SetRecipe(RecipeSO recipeSO)
    {
        this.recipeSO = recipeSO;
        recipeName.text = recipeSO.item.ID;
        icon.sprite = recipeSO.item.icon;

        foreach(KeyValuePair<ItemSO, int> kvp in recipeSO.ingredients)
        {
            IngredientSlot ingredientSlot = Instantiate(ingredientSlotPrefab).GetComponent<IngredientSlot>();
            ingredientSlot.transform.SetParent(ingredientSlotParent.transform);
            ingredientSlot.SetIngredient(kvp.Key, kvp.Value);
            ingredientSlots.Add(ingredientSlot);
        }
    }   
    
    public void UpdateRecipe(List<Item> ingredients, bool canAddItem)
    {
        foreach (IngredientSlot slot in ingredientSlots)
        {
            Item item = ingredients.FirstOrDefault(i => i.info == slot.itemSO);

            if (item == null)
                continue;

            slot.UpdateIngredient(item.amount);
        }

        button.interactable = ingredients.IsNullOrEmpty() || !canAddItem ? false : true;
    }

    public void CraftItem()
    {
        craftingTableUI.CraftItem(recipeSO);
    }
}
