using JUTPS.CharacterBrain;
using JUTPS.JUInputSystem;
using NUnit.Framework;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

public class CraftingTable : MonoBehaviour
{
    [SerializeField] private CraftingType type;
    public List<RecipeSO> recipes {  get; private set; }
    private Logger logger;

    private void Awake()
    {
        logger = LogManager.Instance.AddLogger("Crafting Table", LogLevel.INFO);
        recipes = GlobalInventoryManager.Instance.allRecipes[type].Values.ToList();
    }

    public void AddRecipes(List<RecipeSO> recipes)
    {
        foreach (var recipe in recipes)
        {
            if (!this.recipes.Contains(recipe))
            {
                this.recipes.Add(recipe);
            }
        }
    }

    public List<RecipeSO> EvaluateCraftableRecipes(Inventory inventory)
    {
        List<RecipeSO> recipes = new List<RecipeSO>();
        foreach (RecipeSO recipe in this.recipes)
        {
            if(inventory.CanCraft(recipe).Item1)
            {
                recipes.Add(recipe);
            }
        }
        return recipes;
    }

    public Item CraftItem(Inventory inventory, RecipeSO recipe, int amount = 1)
    {
        Item item = null;

        foreach (KeyValuePair<ItemSO, int> ingredient in recipe.ingredients)
        {
            inventory.RemoveItem(ingredient.Key, ingredient.Value * amount);
        }

        item = new Item(amount, recipe.item);

        if (item == null)
        {
            logger.Error("CraftItem() - Failed to craft item");
        }

        return item;
    }
}