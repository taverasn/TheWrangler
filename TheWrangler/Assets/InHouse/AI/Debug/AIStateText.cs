using TMPro;
using UnityEngine;

public class AIStateText : MonoBehaviour
{
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private TextMeshProUGUI stateText;

    private void Update()
    {
        stateText.text = stateMachine.state.GetType().ToString();
    }
}
