using System.Collections;
using JUTPS.JUInputSystem;
using TMPro;
using Unity.Entities;
using UnityEngine;

public class PickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private int amount;
    [SerializeField] private float hoverHeight = 0.25f;
    [SerializeField] private float hoverSpeed = 1f;
    [SerializeField] private float textVerticalOffset = 1.0f;

    public Inventory inventory { get; set; }
    private Item item;
    private JUTPSInputControlls _inputs;
    private Coroutine _hoverCoroutine;
    private Vector3 _initialPosition;
    private Rigidbody _rigidbody;

    Transform IInteractable.transform { get => transform; set => throw new System.NotImplementedException(); }
    InteractType IInteractable.type { get => InteractType.ItemPickUp; set => throw new System.NotImplementedException(); }

    private void Start()
    {
        item = new Item(amount, itemSO);

        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        Transform existingStick = transform.Find("(Carriable) Stick");
        if (existingStick != null)
        {
            spawnPosition = existingStick.position+=Vector3.up * 0.5f;
            spawnRotation = existingStick.rotation;
            Destroy(existingStick.gameObject);
        }

        Instantiate(itemSO.prefab, spawnPosition, spawnRotation, transform);
        _rigidbody = GetComponent<Rigidbody>();

        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (_hoverCoroutine == null)
        {
            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = true;
            }
            
            _initialPosition = transform.position;
            _hoverCoroutine = StartCoroutine(HoverEffect());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Inventory otherInventory = other.gameObject.GetComponent<Inventory>();
        if (otherInventory != null)
        {
            inventory = otherInventory;

            if (other.CompareTag("Player"))
            {
                popupText.gameObject.SetActive(true);
            }
            else
            {
                PickUpItem();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Inventory otherInventory = other.gameObject.GetComponent<Inventory>();
        if (otherInventory != null)
        {
            if (other.CompareTag("Player"))
            {
                inventory = null;
                popupText.gameObject.SetActive(false);
            }
        }
    }

    private void LateUpdate()
    {
        if (popupText.gameObject.activeInHierarchy)
        {
            Vector3 targetWorldPosition = new Vector3(transform.position.x, transform.position.y + textVerticalOffset * 2, transform.position.z);
            popupText.transform.position = targetWorldPosition;

            popupText.transform.LookAt(popupText.transform.position + Camera.main.transform.rotation * Vector3.forward,
                                     Camera.main.transform.rotation * Vector3.up);
        }
    }

    private IEnumerator HoverEffect()
    {
        while (true)
        {
            var newY = _initialPosition.y + ((Mathf.Sin(Time.time * hoverSpeed) + 1) / 2f) * hoverHeight;
            transform.position = new Vector3(_initialPosition.x, newY, _initialPosition.z);
            yield return null;
        }
    }
}
