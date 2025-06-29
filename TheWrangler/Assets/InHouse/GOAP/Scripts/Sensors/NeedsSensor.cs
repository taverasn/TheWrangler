using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace TheWrangler.GOAP
{
    public class NeedsSensor : MultiSensorBase
    {
        public NeedsSensor()
        {
            // One sensor method per world key
            this.AddLocalWorldSensor<ChargeLow>(SenseChargeLow);
            this.AddLocalWorldSensor<ThirstLow>(SenseThirstLow);
            this.AddLocalWorldSensor<SelfHealthLow>(SenseHealthLow);
            this.AddLocalWorldSensor<InfectionHigh>(SenseInfectionHigh);
        }

        public override void Created() { }

        public override void Update() { }

        private SenseValue SenseChargeLow(IActionReceiver agent, IComponentReference references)
        {
            return SenseNeed(NeedsType.HUNGER, references);
        }

        private SenseValue SenseThirstLow(IActionReceiver agent, IComponentReference references)
        {
            return SenseNeed(NeedsType.THIRST, references);
        }

        private SenseValue SenseHealthLow(IActionReceiver agent, IComponentReference references)
        {
            return SenseNeed(NeedsType.HEALTH, references);
        }

        private SenseValue SenseInfectionHigh(IActionReceiver agent, IComponentReference references)
        {
            return SenseNeed(NeedsType.INFECTION, references); // Higher = worse
        }

        private int SenseNeed(NeedsType type, IComponentReference references)
        {
            NeedsController needsController = references.GetCachedComponent<NeedsController>();
            return needsController.ShouldSatisfyNeed(type);
        }
    }
}
