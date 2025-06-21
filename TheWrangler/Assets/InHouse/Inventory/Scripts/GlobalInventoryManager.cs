using System.Collections.Generic;
using UnityEngine;

public class GlobalInventoryManager : MonoBehaviour
{
    public static GlobalInventoryManager Instance;
    public Dictionary<string, ItemSO> allItemSOs = new Dictionary<string, ItemSO>();
    public Dictionary<CraftingType, Dictionary<string, RecipeSO>> allRecipes = new Dictionary<CraftingType, Dictionary<string, RecipeSO>>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one GlobalInventoryManager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }

        ItemSO[] items = Resources.LoadAll<ItemSO>("Items");
        for (int i = 0; i < items.Length; i++)
        {
            allItemSOs[items[i].ID] = items[i];
        }

        allRecipes.Add(CraftingType.BUILDING, new Dictionary<string, RecipeSO>());
        RecipeSO[] recipes = Resources.LoadAll<RecipeSO>("Recipes/Building");
        for (int i = 0; i < recipes.Length; i++)
        {
            allRecipes[CraftingType.BUILDING][recipes[i].ID] = recipes[i];
        }

        allRecipes.Add(CraftingType.RESOURCES, new Dictionary<string, RecipeSO>());
        recipes = Resources.LoadAll<RecipeSO>("Recipes/CraftedResources");
        for (int i = 0; i < recipes.Length; i++)
        {
            allRecipes[CraftingType.RESOURCES][recipes[i].ID] = recipes[i];
        }

        allRecipes.Add(CraftingType.ARMOR, new Dictionary<string, RecipeSO>());
        recipes = Resources.LoadAll<RecipeSO>("Recipes/Equipment/Armor");
        for (int i = 0; i < recipes.Length; i++)
        {
            allRecipes[CraftingType.ARMOR][recipes[i].ID] = recipes[i];
        }

        allRecipes.Add(CraftingType.WEAPON, new Dictionary<string, RecipeSO>());
        recipes = Resources.LoadAll<RecipeSO>("Recipes/Equipment/Weapon");
        for (int i = 0; i < recipes.Length; i++)
        {
            allRecipes[CraftingType.WEAPON][recipes[i].ID] = recipes[i];
        }

        Instance = this;
    }
}
