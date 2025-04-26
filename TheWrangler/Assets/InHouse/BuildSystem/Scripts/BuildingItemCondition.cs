using EasyBuildSystem.Features.Runtime.Buildings.Part.Conditions;
using UnityEngine;


[BuildingCondition("Building Item Condition",
    "This condition checks if the Inventory has the required items to craft this Building Part", 2)]
public class BuildingItemCondition : BuildingCondition
{
    #region Fields
    [HideInInspector] public Inventory inventory;
    [SerializeField] private RecipeSO m_Recipe;
    public RecipeSO Recipe { get { return m_Recipe; } set { m_Recipe = value; } }

    #endregion

    #region Internal Methods
    public override bool CheckPlacingCondition()
    {
        return Recipe == null || inventory == null ? true : inventory.CanCraft(Recipe);
    }
    #endregion
}
