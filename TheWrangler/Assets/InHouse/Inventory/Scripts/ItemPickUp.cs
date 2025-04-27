using JUTPS.JUInputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class ItemPickUP : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private int amount;

    private Inventory inventory;
    private Item item;
    private JUTPSInputControlls _inputs;

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

    public void OnPickUpItem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (inventory == null) return;

        inventory.AddItem(item);

        Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inventory = other.gameObject.GetComponent<Inventory>();
            popupText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inventory = null;
            popupText.gameObject.SetActive(false);
        }
    }
}
