using JUTPS;
using UnityEngine;

public class ResourceHealth : JUHealth
{
    [field:SerializeField] public ToolType toolType { get; private set; }
    [field:SerializeField] public Tier tier { get; private set; }
    [SerializeField] private ResourceSO resourceDrop;
    [SerializeField] private Transform itemPickUpPrefab;
    private int dropAmount;

    private void Awake()
    {
        dropAmount = Random.Range(0, 100);
    }

    private void OnEnable()
    {
        OnDeath.AddListener(HandleResourceDeath);
    }

    private void OnDisable()
    {
        OnDeath.RemoveListener(HandleResourceDeath);
    }

    private void HandleResourceDeath()
    {
        PickUp pickUp = Instantiate(itemPickUpPrefab, null).GetComponent<PickUp>();
        pickUp.SetUp(resourceDrop, dropAmount);
        Destroy(this.gameObject);
    }
}
