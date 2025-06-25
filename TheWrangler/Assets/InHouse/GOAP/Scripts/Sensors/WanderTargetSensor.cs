using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using TheWrangler.GOAP.Config;
using TheWrangler.GOAP.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace TheWrangler.GOAP.Sensors
{
    public class WanderTargetSensor : LocalTargetSensorBase, IInjectable
    {
        const int MAX_TARGET_SEARCH_ATTEMPTS = 5;
        const int MAX_DISTANCE_FROM_TARGET = 1;

        private WanderConfigSO wanderConfig;

        public override void Created()
        {
        }

        private Vector3 GetRandomPosition(IActionReceiver agent)
        {
            int count = 0;
            while (count < MAX_TARGET_SEARCH_ATTEMPTS)
            {
                Vector2 random = Random.insideUnitCircle * wanderConfig.WanderRadius;
                Vector3 position = agent.Transform.position + new Vector3(
                    random.x,
                    0,
                    random.y
                );
                if (NavMesh.SamplePosition(position, out NavMeshHit hit, MAX_DISTANCE_FROM_TARGET, NavMesh.AllAreas))
                {
                    return hit.position;
                }
                count++;
            }


            return agent.Transform.position;
        }

        public override void Update()
        {
        }

        public void Inject(DependencyInjector dependencyInjector)
        {
            wanderConfig = dependencyInjector.wanderConfig;
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
        {
            Vector3 position = GetRandomPosition(agent);
            return new PositionTarget(position);
        }
    }
}