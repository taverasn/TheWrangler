using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace TheWrangler.GOAP
{
    public class NeedsCapabilityFactory : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder("IdleCapability");

            builder.AddGoal<ChargeGoal>()
                .AddCondition<ChargeLow>(Comparison.SmallerThanOrEqual, 0)
                .SetBaseCost(2);

            builder.AddAction<ChargeAction>()
                .AddEffect<ChargeLow>(EffectType.Decrease)
                .SetTarget<ChargeStationTarget>()
                .SetStoppingDistance(1);

            builder.AddTargetSensor<ChargeStationTargetSensor>()
                .SetTarget<ChargeStationTarget>();

            builder.AddMultiSensor<NeedsSensor>();

            return builder.Build();
        }
    }
}