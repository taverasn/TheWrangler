using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NeedsUI : MonoBehaviour
{
    [SerializeField] private NeedsOwner owner;
    [SerializeField] private GameObject needsParent;
    private Dictionary<NeedsType, NeedsBarUI> needsBars = new Dictionary<NeedsType, NeedsBarUI>();

    private void Awake()
    {
        foreach (NeedsBarUI bar in needsParent.GetComponentsInChildren<NeedsBarUI>())
        {
            needsBars.Add(bar.needsType, bar);
        }
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.NeedsEvents.onBroadcastNeedsUpdate += OnBroadcaseNeedsUpdate;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.NeedsEvents.onBroadcastNeedsUpdate -= OnBroadcaseNeedsUpdate;
    }

    private void OnBroadcaseNeedsUpdate(NeedsBroadcastEvent needsBroadcastEvent)
    {
        if (needsBroadcastEvent.owner == owner && needsBars.TryGetValue(needsBroadcastEvent.need.info.NeedsType, out NeedsBarUI bar))
        {
            switch(needsBroadcastEvent.reason)
            {
                case NeedsBroadcastReason.START_UP:
                    bar.SetSliderValue(needsBroadcastEvent.need.NormalizedValue, true);
                    break;
                case NeedsBroadcastReason.INCREASED:
                case NeedsBroadcastReason.DECREASED:
                    bar.SetSliderValue(needsBroadcastEvent.need.NormalizedValue, false);
                    break;
            }
        }
    }
}
