using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using TheWrangler.GOAP.Behaviour;
using TheWrangler.GOAP.Interfaces;
using UnityEngine;

namespace TheWrangler.GOAP.Sensors
{
    public class ChargeSensor : LocalWorldSensorBase, IInjectable
    {

        public override void Created()
        {
        }

        public void Inject(DependencyInjector dependencyInjector)
        {
        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            return new SenseValue(Mathf.CeilToInt(references.GetCachedComponent<ChargeBehaviour>().charge));
        }

        public override void Update()
        {
        }
    }
}