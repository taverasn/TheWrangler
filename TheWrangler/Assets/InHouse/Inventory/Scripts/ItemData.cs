using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int amount;
    public string ID;

    public ItemData(int amount, string ID)
    {
        this.amount = amount;
        this.ID = ID;
    }
}
