using CrashKonijn.Agent.Runtime;
using UnityEngine;

namespace TheWrangler.GOAP.Behaviour
{
    [RequireComponent(typeof(Animator), typeof(AgentBehaviour))]
    public class ChargeBehaviour : MonoBehaviour
    {
        [field:SerializeField] public float charge { get; set; }
        [SerializeField] private NeedsConfigSO needsConfig;
        private AgentBehaviour agentBehaviour;
        

        private void Awake()
        {
            agentBehaviour = GetComponent<AgentBehaviour>();
            charge = Random.Range(0, needsConfig.MaxNeedsThreshold);
        }

        private void Update()
        {
            charge += Time.deltaTime * needsConfig.NeedsDepletionRate;
        }
    }

}
