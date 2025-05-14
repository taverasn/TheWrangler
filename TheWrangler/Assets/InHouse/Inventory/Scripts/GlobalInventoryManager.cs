using System.Collections.Generic;
using UnityEngine;

public class GlobalInventoryManager : MonoBehaviour
{
    public static GlobalInventoryManager Instance;
    public Dictionary<string, ItemSO> allItemSOs = new Dictionary<string, ItemSO>();
    public Dictionary<string, RecipeSO> allRecipeSOs = new Dictionary<string, RecipeSO>();

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
        
        RecipeSO[] recipes = Resources.LoadAll<RecipeSO>("Recipes");
        for (int i = 0; i < recipes.Length; i++)
        {
            allRecipeSOs[recipes[i].ID] = recipes[i];
        }


        Instance = this;
    }
}
