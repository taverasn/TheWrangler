using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace TheWrangler.GOAP
{
    public class IdleCapabilityFactory : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder("IdleCapability");

            builder.AddGoal<WanderGoal>()
                .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1)
                .SetBaseCost(10);

            builder.AddAction<WanderAction>()
                .AddEffect<IsWandering>(EffectType.Increase)
                .SetTarget<WanderTarget>();

            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();

            return builder.Build();
        }
    }
}