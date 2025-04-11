using NUnit.Framework;
using System.Collections.Generic;

public class CraftingTable
{
    public List<RecipeSO> recipes = new List<RecipeSO>();
    private Logger logger;

    public CraftingTable() 
    {
        logger = LogManager.Instance.AddLogger("Crafting Table", LogLevel.INFO);
    }

    private void HandleOpenTable()
    {

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
            if(inventory.CanCraft(recipe))
            {
                recipes.Add(recipe);
            }
        }
        return recipes;
    }

    public Item CraftItem(Inventory inventory, RecipeSO recipe)
    {
        Item item = null;

        foreach (KeyValuePair<ItemSO, int> ingredient in recipe.ingredients)
        {
            inventory.RemoveItem(ingredient.Key, ingredient.Value);
        }

        item = new Item(1, recipe.item);

        if (item == null)
        {
            logger.Error("CraftItem() - Failed to craft item");
        }

        return item;
    }
}