using CrashKonijn.Goap.Behaviours;
using System;
using TheWrangler.GOAP.Config;
using TheWrangler.GOAP.Goals;
using TheWrangler.GOAP.Sensors;
using UnityEngine;

namespace TheWrangler.GOAP.Behaviour
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class BaseBrain : MonoBehaviour
    {
        [SerializeField] private CharacterSensor characterSensor;
        [SerializeField] private AttackConfigSO attackConfig;
        private AgentBehaviour agentBehaviour;

        private void Awake()
        {
            agentBehaviour = GetComponent<AgentBehaviour>();
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
            agentBehaviour.SetGoal<WanderGoal>(false);

            characterSensor.sphereCollider.radius = attackConfig.SensorRadius;
        }

        private void CharacterSensorOnCharacterEnter(Transform character)
        {
            agentBehaviour.SetGoal<KillTarget>(true);
        }

        private void CharacterSensorOnCharacterExit(Vector3 lastKnownPosition)
        {
            agentBehaviour.SetGoal<WanderGoal>(true);
        }

    }
}
