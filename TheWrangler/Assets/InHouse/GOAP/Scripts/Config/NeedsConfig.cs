using UnityEngine;

[CreateAssetMenu(fileName = "Needs Config", menuName = "AI/Needs Config", order = 3)]
public class NeedsConfigSO : ScriptableObject
{
    public float NeedsSearchRadius = 10f;
    public LayerMask NeedsLayer;
    public float NeedsRestorationRate = 1f;
    public float NeedsDepletionRate = 0.25f;
    public float MaxNeedsThreshold = 20f;
    public float AcceptableNeedsLimit = 10f;
    public float NeedsCheckInterval = 1f;
}
