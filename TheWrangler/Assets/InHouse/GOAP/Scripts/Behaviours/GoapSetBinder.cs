using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace TheWrangler.GOAP.Behaviour
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class GoapSetBinder : MonoBehaviour
    {
        [SerializeField] private GoapRunnerBehaviour goapRunner;

        private void Awake()
        {
            AgentBehaviour agent = GetComponent<AgentBehaviour>();
            agent.GoapSet = goapRunner.GetGoapSet("BaseSet");
        }
    }
}
