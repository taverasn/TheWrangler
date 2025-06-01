using UnityEngine;

public interface IInteractable
{
    public Transform transform { get; set; }
    public Inventory inventory { get; set; }
}