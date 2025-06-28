using UnityEngine;

public class Need
{
    public NeedsSO info { get; private set; }
    private NeedsOwner owner;
    public float maxValue { get; private set; }
    public float currentValue {get; private set;}
    private float depletionRate;
    private float restorationRate;

    private float dotTimer;
    private float dotRate;

    private bool canRestore;
    // TODO: At the moment this is a debug bool to be able to play without worrying about needs management
    private bool canDeplete = true;

    // Public State Properties
    public float NormalizedValue => maxValue == 0 ? 0 : currentValue / maxValue;
    public bool HasActiveDot => dotTimer > 0 && dotRate != 0;
    public bool IsDepleted => info.DepletionType == DepletionType.EMPTY ? currentValue <= 0f : currentValue >= maxValue;
    public bool IsFull => info.DepletionType == DepletionType.EMPTY ?  currentValue >= maxValue : currentValue <= 0f;

    // Internal Conditions
    bool ShouldRestore => (info.DepletionType == DepletionType.EMPTY ?
                        currentValue < maxValue :
                        currentValue > 0) &&
                        restorationRate > 0 &&
                        canRestore;

    bool ShouldDeplete => (info.DepletionType == DepletionType.EMPTY ?
                            currentValue > 0 :
                            currentValue < maxValue) &&
                            depletionRate > 0 &&
                            !canRestore;

    public Need(NeedsSO info, NeedsOwner owner)
    {
        this.info = info;
        this.owner = owner;
        this.maxValue = info.MaxValue;
        this.currentValue = info.DepletionType == DepletionType.FILL ? info.MaxValue : 0;
        this.depletionRate = info.DepletionRate;
        this.restorationRate = info.RestorationRate;
        this.canRestore = restorationRate > 0;
    }

    public Need(NeedsSO info, NeedsOwner owner, float maxValue, float currentValue, float depletionRate, float restorationRate)
    {
        this.info = info;
        this.owner = owner;
        this.maxValue = maxValue;
        this.currentValue = currentValue;
        this.depletionRate = depletionRate;
        this.restorationRate = restorationRate;
        this.canRestore = restorationRate > 0;
    }

    public void Update(float deltaTime, bool isOutOfCombat)
    {
        if (ShouldRestore && isOutOfCombat)
            ChangeCurrentValue(restorationRate * deltaTime);

        if (ShouldDeplete)
            ChangeCurrentValue(-depletionRate * deltaTime);

        if (dotTimer > 0 && dotRate != 0)
        {
            dotTimer -= deltaTime;
            ChangeCurrentValue(dotRate);

            if (dotTimer < 0)
            {
                ClearEffectOverTime();
            }
        }
    }

    public void TickDepletion(float deltaTime)
    {
        if (ShouldDeplete)
            ChangeCurrentValue(-this.depletionRate * deltaTime);
    }

    public void ApplyEffectOverTime(float dotRate, float dotTimer)
    {
        this.dotRate = dotRate;
        this.dotTimer = dotTimer;
        GameEventsManager.Instance.NeedsEvents.BroadcastNeedsUpdate(owner, this, NeedsBroadcastReason.DOT_STARTED);
    }

    public void ClearEffectOverTime()
    {
        this.dotRate = 0;
        this.dotTimer = 0;
        GameEventsManager.Instance.NeedsEvents.BroadcastNeedsUpdate(owner, this, NeedsBroadcastReason.DOT_ENDED);
    }

    public void ChangeCurrentValue(float value)
    {
        if (!canDeplete)
            return;

        // If value is positive at this point that means that we're allowed to naturally restore
        canRestore = value > 0;

        float changeValue = info.DepletionType == DepletionType.FILL ? value : -value;

        this.currentValue = Mathf.Clamp(this.currentValue + changeValue, 0, maxValue);

        NeedsBroadcastReason reason = NeedsBroadcastReason.UNKNOWN;
        
        if (value > 0)
            reason = IsFull ? NeedsBroadcastReason.REACHED_MAXIMUM : NeedsBroadcastReason.INCREASED;
        else if (value < 0)
            reason = IsDepleted ? NeedsBroadcastReason.REACHED_MINIMUM : NeedsBroadcastReason.DECREASED;

        GameEventsManager.Instance.NeedsEvents.BroadcastNeedsUpdate(owner, this, reason);
    }

    public void UpgradeMaxValue(float value)
    {
        maxValue += value;
    }

    public void UpgradeDepletionRate(float value)
    {
        depletionRate -= value;
    }

    public void UpgradeRestorationRate(float value)
    {
        restorationRate += value;
    }

    public void SetCanDeplete(bool canDeplete)
    {
        this.canDeplete = canDeplete;
    }

    public NeedsData GetNeedsData()
    {
        return new NeedsData(maxValue, currentValue, depletionRate, restorationRate);
    }
}
