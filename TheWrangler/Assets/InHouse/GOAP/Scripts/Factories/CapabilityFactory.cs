using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using TheWrangler.GOAP.Actions;
using TheWrangler.GOAP.Goals;
using TheWrangler.GOAP.Sensors;
using TheWrangler.GOAP.Targets;
using TheWrangler.GOAP.WorldKeys;
using UnityEngine;

namespace TheWrangler.GOAP.Factories
{
    [RequireComponent(typeof(DependencyInjector))]
    public class CapabilityFactory : MonoCapabilityFactoryBase
    {
        private DependencyInjector injector;

        public override ICapabilityConfig Create()
        {
            injector = GetComponent<DependencyInjector>();
            CapabilityBuilder builder = new("BaseSet");

            BuildGoals(builder);
            BuildActions(builder);
            BuildSensors(builder);

            return builder.Build();
        }

        private void BuildGoals(CapabilityBuilder builder)
        {
            builder.AddGoal<WanderGoal>()
                .SetBaseCost(50)
                .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);
            
            builder.AddGoal<KillTarget>()
                .AddCondition<TargetHealth>(Comparison.SmallerThanOrEqual, 0);

            builder.AddGoal<ChargeGoal>()
                .AddCondition<Charge>(Comparison.SmallerThanOrEqual, 0);
        }

        private void BuildActions(CapabilityBuilder builder)
        {
            builder.AddAction<WanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWandering>(EffectType.Increase)
                .SetBaseCost(5)
                .SetStoppingDistance(10);

            builder.AddAction<MeleeAction>()
                .SetTarget<CharacterTarget>()
                .AddEffect<TargetHealth>(EffectType.Decrease)
                .SetBaseCost(injector.attackConfig.MeleeAttackCost)
                .SetStoppingDistance(injector.attackConfig.SensorRadius);

            builder.AddAction<ChargeAction>()
                .SetTarget<ChargeStationTarget>()
                .AddEffect<Charge>(EffectType.Decrease)
                .SetBaseCost(8)
                .SetStoppingDistance(1);
        }

        private void BuildSensors(CapabilityBuilder builder)
        {
            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();

            builder.AddTargetSensor<CharacterTargetSensor>()
                .SetTarget<CharacterTarget>();

            builder.AddTargetSensor<ChargeStationTargetSensor>()
                .SetTarget<ChargeStationTarget>();

            builder.AddWorldSensor<ChargeSensor>()
                .SetKey<Charge>();
        }
    }
}
