using JUTPS.JUInputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class PickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private int amount;
    public Inventory inventory { get; set; }
    private Item item;
    private JUTPSInputControlls _inputs;

    Transform IInteractable.transform { get => transform; set => throw new System.NotImplementedException(); }
    InteractType IInteractable.type { get => InteractType.ItemPickUp; set => throw new System.NotImplementedException(); }

    private void Start()
    {
        item = new Item(amount, itemSO);
    }

    private void OnEnable()
    {
        _inputs = JUInput.Instance().InputActions;
        _inputs.Player.Interact.started += OnPickUpItem;
    }

    private void OnDisable()
    {
        _inputs.Player.Interact.started -= OnPickUpItem;
    }

    public void SetUp(ItemSO _itemSO, int _amount)
    {
        item = new Item(_amount, _itemSO);
    }

    public void PickUpItem()
    {
        if (inventory == null) return;

        inventory.AddItem(item);

        Destroy(this.gameObject);
    }

    public void OnPickUpItem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        PickUpItem();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Inventory>() != null)
        {
            inventory = other.gameObject.GetComponent<Inventory>();

            if (other.CompareTag("Player"))
            {
                popupText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inventory = null;
            if (other.CompareTag("Player"))
            {
                popupText.gameObject.SetActive(false);
            }
        }
    }
}
