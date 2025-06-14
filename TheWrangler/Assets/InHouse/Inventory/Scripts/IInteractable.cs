using EasyBuildSystem.Packages.Addons.AdvancedBuilding;
using UnityEngine;

public interface IInteractable
{
    public Transform transform { get; set; }
    public Inventory inventory { get; set; }
    public InteractType type { get; set; }
}

public enum InteractType
{
    None,
    Inventory,
    ItemPickUp
}