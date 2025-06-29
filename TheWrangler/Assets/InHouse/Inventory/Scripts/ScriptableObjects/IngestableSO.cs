using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableSO", menuName = "Items/ConsumableSO")]
[Serializable]
public class ConsumableSO : ItemSO
{
    [Header("Consumable Settings")]
    public NeedsType type;

    public NeedsUpdateReason reason;

    public float value;

    public float timer;

    public void NeedsUpdateMessage(NeedsOwner owner, string guid, NeedsUpdateMessageOverride needsOverride = NeedsUpdateMessageOverride.NONE, float valueModifier = 0f, float timerModifier = 0f, NeedsUpdateReason reasonModifier = NeedsUpdateReason.NONE)
    {
        NeedsUpdateEvent needsUpdateEvent = new NeedsUpdateEvent
        {
            owner = owner,
            guid = guid,
            type = type,
        };

        switch(needsOverride)
        {
            case NeedsUpdateMessageOverride.NONE:
                needsUpdateEvent.reason = reason;
                needsUpdateEvent.value = value;
                needsUpdateEvent.timer = timer;
                break;
            case NeedsUpdateMessageOverride.MODIFY_VALUES_INCREASE:
                needsUpdateEvent.reason = reasonModifier;
                needsUpdateEvent.value = value + valueModifier;
                needsUpdateEvent.timer = timer + valueModifier;
                break;
            case NeedsUpdateMessageOverride.MODIFY_VALUES_DECREASE:
                needsUpdateEvent.reason = reasonModifier;
                needsUpdateEvent.value = value - valueModifier;
                timer = timer - valueModifier;
                break;
            case NeedsUpdateMessageOverride.MODIFY_VALUES_PERCENTAGE:
                needsUpdateEvent.reason = reasonModifier;
                needsUpdateEvent.value = value * valueModifier;
                needsUpdateEvent.timer = timer * valueModifier;
                break;
            case NeedsUpdateMessageOverride.OVERRIDE_VALUES:
                needsUpdateEvent.reason = reasonModifier;
                needsUpdateEvent.value = valueModifier;
                needsUpdateEvent.timer = valueModifier;
                break;
        }

        GameEventsManager.Instance.NeedsEvents.UpdateNeeds(needsUpdateEvent);
    }
}

public enum NeedsUpdateMessageOverride
{
    NONE,
    MODIFY_VALUES_INCREASE,
    MODIFY_VALUES_DECREASE,
    MODIFY_VALUES_PERCENTAGE,
    OVERRIDE_VALUES,
}
