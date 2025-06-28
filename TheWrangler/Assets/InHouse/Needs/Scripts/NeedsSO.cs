using UnityEngine;

[CreateAssetMenu(fileName = "NeedsSO", menuName = "Needs/NeedsSO")]
public class NeedsSO : ScriptableObject
{
    public string Name;
    public float MaxValue;
    public NeedsType NeedsType;
    public DepletionType DepletionType;
    public DepletionBehaviour DepletionBehaviour;
    public float DepletionRate;
    public float RestorationRate;
}

