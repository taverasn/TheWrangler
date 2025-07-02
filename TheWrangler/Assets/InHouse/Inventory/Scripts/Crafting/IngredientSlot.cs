using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI displayName;
    [SerializeField] private TextMeshProUGUI amount;
    private int amountNeeded;
    public bool amountReached {get; private set;}
    public ItemSO itemSO { get; private set; }
    public void SetIngredient(ItemSO item, int amountNeeded)
    {
        icon.sprite = item.icon;
        displayName.text = item.displayName;
        this.amountNeeded = amountNeeded;
        amount.text = $"0/{amountNeeded.ToString()}";
        itemSO = item;
    }

    public void UpdateIngredient(int currentAmount)
    {
        amount.text = $"{currentAmount.ToString()}/{amountNeeded.ToString()}";

        if (currentAmount >= amountNeeded)
        {
            amountReached = true;
            amount.color = Color.green;
        }
        else
        {
            amountReached = false;
            amount.color = Color.red;
        }

    }
}
