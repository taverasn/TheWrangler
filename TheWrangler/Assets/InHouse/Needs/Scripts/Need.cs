using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Need
{
    public NeedsSO info { get; private set; }
    private NeedsOwner owner;
    private string guid;
    public float maxValue { get; private set; }
    public float currentValue {get; private set;}
    private float depletionRate;
    private float restorationRate;

    private float dotTimer;
    private float dotRate;
    public GOTO goTo {get; private set;}
    private float elapsedTime;
    private float reachValue;

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
                        goTo == GOTO.NONE;

    bool ShouldDeplete => (info.DepletionType == DepletionType.EMPTY ?
                            currentValue > 0 :
                            currentValue < maxValue) &&
                            depletionRate > 0 &&
                            info.DepletionBehaviour == DepletionBehaviour.PASSIVE &&
                            goTo == GOTO.NONE;

    public Need(NeedsSO info, NeedsOwner owner, string guid)
    {
        this.info = info;
        this.owner = owner;
        this.guid = guid;
        this.maxValue = info.MaxValue;
        this.currentValue = info.DepletionType == DepletionType.EMPTY ? info.MaxValue : 0;
        this.depletionRate = info.DepletionRate;
        this.restorationRate = info.RestorationRate;
    }

    public Need(NeedsSO info, NeedsOwner owner, string guid, float maxValue, float currentValue, float depletionRate, float restorationRate)
    {
        this.info = info;
        this.owner = owner;
        this.guid = guid;
        this.maxValue = maxValue;
        this.currentValue = currentValue;
        this.depletionRate = depletionRate;
        this.restorationRate = restorationRate;
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

        if (goTo != GOTO.NONE)
        {
            if (elapsedTime < dotTimer && currentValue != reachValue)
            {
                elapsedTime += deltaTime;
                float t = Mathf.Clamp01(elapsedTime / dotTimer);
                NeedsBroadcastReason reason = NeedsBroadcastReason.UNKNOWN;
                currentValue = Mathf.Lerp(currentValue, reachValue, t);
                if (goTo == GOTO.MAXIMUM)
                    reason = IsFull ? NeedsBroadcastReason.REACHED_MAXIMUM : NeedsBroadcastReason.INCREASED;
                else if (goTo == GOTO.MINIMUM)
                    reason = IsDepleted ? NeedsBroadcastReason.REACHED_MINIMUM : NeedsBroadcastReason.DECREASED;

                GameEventsManager.Instance.NeedsEvents.BroadcastNeedsUpdate(new NeedsBroadcastEvent(owner, guid.ToString(), this, reason));
            }
            else
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
        GameEventsManager.Instance.NeedsEvents.BroadcastNeedsUpdate(new NeedsBroadcastEvent(owner, guid.ToString(), this, NeedsBroadcastReason.DOT_STARTED));
    }

    public void ApplyEffectOverTime(float dotTimer, bool maximum = true)
    {
        this.dotTimer = dotTimer;

        if (maximum)
            reachValue = info.DepletionType == DepletionType.EMPTY ? info.MaxValue : 0;
        else
            reachValue = info.DepletionType == DepletionType.EMPTY ? 0 : info.MaxValue;

        goTo = maximum ? GOTO.MAXIMUM : GOTO.MINIMUM;

        GameEventsManager.Instance.NeedsEvents.BroadcastNeedsUpdate(new NeedsBroadcastEvent(owner, guid.ToString(), this, NeedsBroadcastReason.DOT_STARTED));
    }

    public void ClearEffectOverTime()
    {
        this.dotRate = 0;
        this.dotTimer = 0;
        this.reachValue = 0;
        this.goTo = GOTO.NONE;
        this.elapsedTime = 0;
        GameEventsManager.Instance.NeedsEvents.BroadcastNeedsUpdate(new NeedsBroadcastEvent(owner, guid.ToString(), this, NeedsBroadcastReason.DOT_ENDED));
    }

    public void ChangeCurrentValue(float value)
    {
        if (!canDeplete)
            return;

        float changeValue = info.DepletionType == DepletionType.FILL ? -value : value;

        this.currentValue = Mathf.Clamp(this.currentValue + changeValue, 0, maxValue);

        NeedsBroadcastReason reason = NeedsBroadcastReason.UNKNOWN;
        
        if (value > 0)
            reason = IsFull ? NeedsBroadcastReason.REACHED_MAXIMUM : NeedsBroadcastReason.INCREASED;
        else if (value < 0)
            reason = IsDepleted ? NeedsBroadcastReason.REACHED_MINIMUM : NeedsBroadcastReason.DECREASED;

        GameEventsManager.Instance.NeedsEvents.BroadcastNeedsUpdate(new NeedsBroadcastEvent(owner, guid.ToString(), this, reason));
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

    public void Reset()
    {
        this.currentValue = info.DepletionType == DepletionType.EMPTY ? info.MaxValue : 0;
        this.dotRate = 0;
        this.dotTimer = 0;
        this.canDeplete = true;
    }

    public void SetMinimum()
    {
        this.currentValue = info.DepletionType == DepletionType.EMPTY ? info.MaxValue : 0;
    }

    public void SetMaximum()
    {
        this.currentValue = info.DepletionType == DepletionType.EMPTY ? 0 : info.MaxValue;
    }

    public NeedsData GetNeedsData()
    {
        return new NeedsData(maxValue, currentValue, depletionRate, restorationRate);
    }
}

public enum GOTO
{
    NONE,
    MAXIMUM,
    MINIMUM
}