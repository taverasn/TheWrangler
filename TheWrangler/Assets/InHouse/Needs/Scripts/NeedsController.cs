using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class NeedsController : MonoBehaviour, IDataPersistence
{
    private Dictionary<NeedsType, Need> needsMap = new Dictionary<NeedsType, Need>();
    [SerializeField] private NeedsOwner needsOwner = NeedsOwner.PLAYER;
    // Only owners that are not the Player or Companion care about this since other owners
    // can have multiple of the same type
    public string guid { get; private set; } = "";

    private bool TryGetNeed(NeedsType type, out Need need) => needsMap.TryGetValue(type, out need);
    private bool UsesSaveDataForCurrentValue => needsOwner == NeedsOwner.PLAYER || needsOwner == NeedsOwner.COMPANION;

    // Find a way to get these state variables from the owners controller
    private bool isSprinting;
    private bool isOutOfCombat = true;
    private bool isIdle = true;

    private void Awake()
    {
        if (!UsesSaveDataForCurrentValue)
        {
            guid = Guid.NewGuid().ToString();
        }
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.NeedsEvents.onUpdateNeeds += OnUpdateNeeds;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.NeedsEvents.onUpdateNeeds -= OnUpdateNeeds;
    }

    private void Start()
    {
        foreach (Need need in needsMap.Values)
        {
            GameEventsManager.Instance.NeedsEvents.BroadcastNeedsUpdate(new NeedsBroadcastEvent(needsOwner, guid, need, NeedsBroadcastReason.START_UP));
        }
    }

    public int ShouldSatisfyNeed(NeedsType needsType)
    {
        int satisfyNeed = 0;

        if (TryGetNeed(needsType, out Need need))
        {
            switch (need.info.DepletionType)
            {
                case DepletionType.EMPTY:
                    satisfyNeed = need.NormalizedValue <= 0.5f ? 1 : 0;
                    break;
                case DepletionType.FILL:
                    satisfyNeed = need.NormalizedValue >= 0.5f ? 1 : 0;
                    break;
            }
        }

        return satisfyNeed;
    }

    private void OnUpdateNeeds(NeedsUpdateEvent needsUpdateEvent)
    {
        if (this.needsOwner == needsUpdateEvent.owner && UsesSaveDataForCurrentValue || guid == needsUpdateEvent.guid)
        {
            Need need;

            if (!TryGetNeed(needsUpdateEvent.type, out need))
                throw new Exception($"{needsOwner} NeedsController does not use Need of type: {needsUpdateEvent.type}");

            switch (needsUpdateEvent.reason)
            {
                case NeedsUpdateReason.UPGRADE_MAX_VALUE:
                    need.UpgradeMaxValue(needsUpdateEvent.value);
                    break;
                case NeedsUpdateReason.UPGRADE_DEPLETION_RATE:
                    need.UpgradeDepletionRate(needsUpdateEvent.value);
                    break;
                case NeedsUpdateReason.UPGRADE_RESTORATION_RATE:
                    need.UpgradeRestorationRate(needsUpdateEvent.value);
                    break;
                case NeedsUpdateReason.INCREASE:
                    need.ChangeCurrentValue(needsUpdateEvent.value);
                    break;
                case NeedsUpdateReason.DECREASE:
                    need.ChangeCurrentValue(-needsUpdateEvent.value);
                    break;
                case NeedsUpdateReason.EFFECT_OVER_TIME_INCREASE:
                    need.ApplyEffectOverTime(needsUpdateEvent.value, needsUpdateEvent.timer);
                    break;
                case NeedsUpdateReason.EFFECT_OVER_TIME_DECREASE:
                    need.ApplyEffectOverTime(-needsUpdateEvent.value, needsUpdateEvent.timer);
                    break;
                case NeedsUpdateReason.STOP_DEPLETION:
                    need.SetCanDeplete(false);
                    break;
                case NeedsUpdateReason.RESUME_DEPLETION:
                    need.SetCanDeplete(true);
                    break;
                case NeedsUpdateReason.RESET:
                    need.Reset();
                    break;
                case NeedsUpdateReason.SET_MAXIMUM:
                    break;
                case NeedsUpdateReason.SET_MINIMUM:
                    break;
            }
        }
    }

    private void Update()
    {
        foreach (Need need in needsMap.Values)
        {
            need.Update(Time.smoothDeltaTime, isOutOfCombat);
        }

        if (isSprinting && TryGetNeed(NeedsType.STAMINA, out Need staminaNeed))
        {
            staminaNeed.TickDepletion(Time.smoothDeltaTime);
        }
    }

    public virtual void LoadData(GameData data)
    {
        // Create the quest map
        needsMap.Clear();

        NeedsSO[] allNeeds = Resources.LoadAll<NeedsSO>($"Needs/{needsOwner}");

        if (!data.needs.ContainsKey(needsOwner))
            data.needs.Add(needsOwner, new SerializableDictionary<NeedsType, string>());

        Dictionary<NeedsType, Need> idToNeedsMap = new Dictionary<NeedsType, Need>();
        foreach (NeedsSO needsSO in allNeeds)
        {
            if (idToNeedsMap.ContainsKey(needsSO.NeedsType))
            {
                Debug.LogWarning($"Duplicate ID found when creating {needsOwner} needs map: {needsSO.Name}");
            }
            idToNeedsMap.Add(needsSO.NeedsType, LoadNeed(needsSO, data));
        }
        needsMap = idToNeedsMap;
    }

    public virtual void SaveData(GameData data)
    {
        if (!UsesSaveDataForCurrentValue)
            return;

        if (!data.needs.ContainsKey(needsOwner))
            data.needs.Add(needsOwner, new SerializableDictionary<NeedsType, string>());

        foreach (Need need in needsMap.Values)
        {
            if (data.needs[needsOwner].ContainsKey(need.info.NeedsType))
            {
                data.needs[needsOwner].Remove(need.info.NeedsType);
            }
            data.needs[needsOwner].Add(need.info.NeedsType, SerializedNeedString(need));
        }
    }

    protected string SerializedNeedString(Need need)
    {
        string serializedData = "";
        try
        {
            if (need != null)
            {
                NeedsData needsData = need.GetNeedsData();
                // serialize using JsonUtility
                serializedData = JsonUtility.ToJson(needsData);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save need with id " + need == null ? "DoesNotExist" : need.info.Name + ": " + e);
        }
        return serializedData;
    }

    protected Need LoadNeed(NeedsSO needsSO, GameData data)
    {
        Need need = null;
        try
        {
            // load need from saved data
            if (UsesSaveDataForCurrentValue && data.needs[needsOwner].TryGetValue(needsSO.NeedsType, out string serializedData))
            {
                NeedsData needsData = JsonUtility.FromJson<NeedsData>(serializedData);

                need = new Need(needsSO, needsOwner, guid, needsData.maxValue, needsData.currentValue, needsData.depletionRate, needsData.restorationRate);
            }
            // otherwise, initialize a new need
            else
            {
                need = new Need(needsSO, needsOwner, guid);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load " + needsSO.Name + ": " + e);
        }
        return need;
    }
}

public struct NeedsUpdateEvent
{
    public NeedsOwner owner;
    public string guid;
    public NeedsType type;
    public NeedsUpdateReason reason;
    public float value;
    public float timer;

    public NeedsUpdateEvent(NeedsOwner owner, string guid, NeedsType type, NeedsUpdateReason reason, float value, float timer)
    {
        this.owner = owner;
        this.guid = guid;
        this.type = type;
        this.reason = reason;
        this.value = value;
        this.timer = timer;
    }
}

public struct NeedsBroadcastEvent
{
    public NeedsOwner owner;
    public string guid;
    public Need need;
    public NeedsBroadcastReason reason;

    public NeedsBroadcastEvent(NeedsOwner owner, string guid, Need need, NeedsBroadcastReason reason)
    {
        this.owner = owner;
        this.guid = guid;
        this.need = need;
        this.reason = reason;
    }
}

public enum NeedsType
{
    NONE,
    ALL,
    HEALTH,
    STAMINA,
    HUNGER,
    THIRST,
    INFECTION
}

public enum NeedsOwner
{
    PLAYER,
    COMPANION,
    // The following types dont use data persistence in the same way.
    // They will be used for NPC and Enemy types and provide a current scaling, for their type
    // rather than current value. CurrentValue for these types will always be set to max at start up
    // since we dont want all entities of that type to start at 1 current if only 1 is at that value.
    // Also I dont care/think it matters for these types to save current values.
    ENEMY_ZOMBIE,
    NPC_VILLAGER,
    RESOURCE,
    DESTRUCTABLE_OBJECT,
    NONE,
}

public enum NeedsUpdateReason
{
    UPGRADE_MAX_VALUE,
    UPGRADE_DEPLETION_RATE,
    UPGRADE_RESTORATION_RATE,
    DECREASE,
    INCREASE,
    EFFECT_OVER_TIME_DECREASE,
    EFFECT_OVER_TIME_INCREASE,
    STOP_DEPLETION,
    RESUME_DEPLETION,
    RESET,
    SET_MINIMUM,
    SET_MAXIMUM,
}

public enum DepletionType
{
    // Depletes in Positive Direction (Ex. Overheat)
    FILL,
    // Depletes in Negative Direction (Ex. Hunger)
    EMPTY,
}

// Not currently in use but I like the idea of this since all the behaviours fit into these 3 categories
public enum DepletionBehaviour
{
    // Needs Owner is actively doing something to deplete
    ACTIVE,
    // Depletes overtime
    PASSIVE,
    // Something happened to the Needs Owner for depletion
    ACTION
}

public enum NeedsBroadcastReason
{
    UNKNOWN,
    START_UP,
    INCREASED,
    DECREASED,
    DOT_STARTED,
    DOT_ENDED,
    REACHED_MINIMUM,
    REACHED_MAXIMUM
}