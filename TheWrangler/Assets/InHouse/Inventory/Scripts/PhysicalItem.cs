using UnityEngine;

public class PhysicalItem : MonoBehaviour
{
    public ItemSO itemSO;
    [HideInInspector] public NeedsOwner owner; 
    [HideInInspector] public string guid = ""; 
}
