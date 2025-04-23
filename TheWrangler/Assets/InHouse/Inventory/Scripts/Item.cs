using System;

[Serializable]
public class Item
{
    public int amount { get; set; }
    public ItemSO info { get; set; }

    public Item(int amount, ItemSO info)
    {
        this.amount = amount;
        this.info = info;
    }

    public void Add(int _amount)
    {
        amount += _amount;
    }

    public void Remove(int _amount)
    {
        amount -= _amount;
    }

    public virtual void Use()
    {

    }
}