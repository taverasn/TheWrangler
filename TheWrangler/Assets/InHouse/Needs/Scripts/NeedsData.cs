using System;

[Serializable]
public class NeedsData
{
    public float maxValue;
    public float currentValue;
    public float depletionRate;
    public float restorationRate;

    public NeedsData(float maxValue, float currentValue, float depletionRate, float restorationRate)
    {
        this.maxValue = maxValue;
        this.currentValue = currentValue;
        this.depletionRate = depletionRate;
        this.restorationRate = restorationRate;
    }
}