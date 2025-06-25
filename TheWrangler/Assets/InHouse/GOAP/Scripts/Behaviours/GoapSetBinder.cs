using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace TheWrangler.GOAP.Behaviour
{
    [RequireComponent(typeof(AgentBehaviour), typeof(GoapActionProvider))]
    public class GoapSetBinder : MonoBehaviour
    {
        [SerializeField] private GoapBehaviour goapRunner;

        private void Awake()
        {
            AgentBehaviour agent = GetComponent<AgentBehaviour>();
            GoapActionProvider actionProvider = GetComponent<GoapActionProvider>();
            actionProvider.AgentType = goapRunner.GetAgentType("BaseAgent");
        }
    }
}
