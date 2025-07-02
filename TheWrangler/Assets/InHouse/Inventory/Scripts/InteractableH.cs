using JUTPS.CharacterBrain;
using JUTPS.JUInputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

public class InteractableH : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupText;
    private JUTPSInputControlls _inputs;
    private JUCharacterBrain JUCharacter;

    private void OnEnable()
    {
        _inputs = JUInput.Instance().InputActions;
        _inputs.Player.Interact.started += OnOpenInventory;
    }

    private void OnDisable()
    {
        _inputs.Player.Interact.started -= OnOpenInventory;
    }

    public void OnOpenInventory(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (JUCharacter == null) return;
        GameEventsManager.Instance.UIEvents.OpenPlayerInventory();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            JUCharacter = other.gameObject.GetComponent<JUCharacterBrain>();
            popupText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            JUCharacter = null;
            popupText.gameObject.SetActive(false);
        }
    }
}
