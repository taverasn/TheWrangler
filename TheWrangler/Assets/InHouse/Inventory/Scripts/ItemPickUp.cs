using JUTPS.JUInputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class ItemPickUP : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private Collider collider;
    [SerializeField] private TextMeshProUGUI popupText;
    private Inventory inventory;
    private Item item;
    private JUTPSInputControlls _inputs;

    private void Start()
    {
        item = new Item(1, itemSO);
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
