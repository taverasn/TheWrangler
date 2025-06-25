using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using TheWrangler.GOAP.Config;
using TheWrangler.GOAP.Goals;
using TheWrangler.GOAP.Sensors;
using UnityEngine;

namespace TheWrangler.GOAP.Behaviour
{
    [RequireComponent(typeof(AgentBehaviour), typeof(GoapActionProvider))]
    public class BaseBrain : MonoBehaviour
    {
        [SerializeField] private CharacterSensor characterSensor;
        [SerializeField] private AttackConfigSO attackConfig;
        [SerializeField] private NeedsConfigSO needsConfig;
        [SerializeField] private ChargeBehaviour chargeBehaviour;
        private AgentBehaviour agentBehaviour;
        private GoapActionProvider actionProvider;
        private GoapBehaviour goap;
        private bool playerIsInRange;

        private void Awake()
        {
            agentBehaviour = GetComponent<AgentBehaviour>();
            actionProvider = GetComponent<GoapActionProvider>();
        }

        private void OnEnable()
        {
            characterSensor.OnCharacterEnter += CharacterSensorOnCharacterEnter;   
            characterSensor.OnCharacterExit += CharacterSensorOnCharacterExit;   
        }

        private void OnDisable()
        {
            characterSensor.OnCharacterEnter -= CharacterSensorOnCharacterEnter;
            characterSensor.OnCharacterExit -= CharacterSensorOnCharacterExit;
        }

        private void Start()
        {
            actionProvider.RequestGoal<WanderGoal>();

            characterSensor.sphereCollider.radius = attackConfig.SensorRadius;
        }

        private void Update()
        {
            SetGoal();
        }

        private void SetGoal()
        {
            if (chargeBehaviour.charge > needsConfig.MaxNeedsThreshold) 
            {
                actionProvider.RequestGoal<ChargeGoal>(true);
            }
            else if (chargeBehaviour.charge < needsConfig.AcceptableNeedsLimit && actionProvider.CurrentPlan?.Goal is ChargeGoal && playerIsInRange)
            {
                actionProvider.RequestGoal<KillTarget>(false);
            }
            else if (chargeBehaviour.charge <= 0 && actionProvider.CurrentPlan?.Goal is ChargeGoal && !playerIsInRange)
            {
                actionProvider.RequestGoal<WanderGoal>(false);
            }
        }

        private void CharacterSensorOnCharacterEnter(Transform character)
        {
            playerIsInRange = true;
            SetGoal();
        }

        private void CharacterSensorOnCharacterExit(Vector3 lastKnownPosition)
        {
            playerIsInRange = false;
            SetGoal();
        }

    }
}
