using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using TheWrangler.GOAP.Config;
using TheWrangler.GOAP.Interfaces;
using UnityEngine;

namespace TheWrangler.GOAP.Sensors
{
    public class CharacterTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private AttackConfigSO attackConfig;
        private Collider[] colliders = new Collider[1];
        
        public override void Created()
        {
        }

        public void Inject(DependencyInjector dependencyInjector)
        {
            attackConfig = dependencyInjector.attackConfig;
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            if (Physics.OverlapSphereNonAlloc(agent.transform.position, attackConfig.SensorRadius, colliders, attackConfig.AttackableLayerMask) > 0)
            {
                return new TransformTarget(colliders[0].transform);
            }

            return null;
        }

        public override void Update()
        {
        }
    }
}