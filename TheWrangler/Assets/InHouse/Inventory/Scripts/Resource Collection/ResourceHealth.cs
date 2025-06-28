using JUTPS;
using JUTPS.FX;
using JUTPSEditor.JUHeader;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ResourceHealth : JUHealth
{
    [field: SerializeField] public ToolType toolType { get; private set; }
    [field: SerializeField] public Tier tier { get; private set; }
    [SerializeField] private ResourceSO resourceDrop;
    [SerializeField] private Transform itemPickUpPrefab;
    private int dropAmount;

    private void Awake()
    {
        dropAmount = Random.Range(0, 100);
    }


    protected override void OnEnable()
    {
        GameEventsManager.Instance.NeedsEvents.onBroadcastNeedsUpdate += OnBroadcastNeedsUpdate;
        OnDestroyed.AddListener(HandleResourceDeath);
    }

    protected override void OnDisable()
    {
        GameEventsManager.Instance.NeedsEvents.onBroadcastNeedsUpdate -= OnBroadcastNeedsUpdate;
        OnDestroyed.RemoveListener(HandleResourceDeath);
    }

    private void HandleResourceDeath()
    {
        PickUp pickUp = Instantiate(itemPickUpPrefab).GetComponent<PickUp>();
        pickUp.SetUp(resourceDrop, dropAmount);
        pickUp.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }
}
