using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSO", menuName = "Items/ResourceSO")]
[Serializable]
public class ResourceSO : ItemSO
{
    public Tier tier;
    public ToolType toolType;
}


