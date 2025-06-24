using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using TheWrangler.GOAP.Actions;
using TheWrangler.GOAP.Goals;
using TheWrangler.GOAP.Sensors;
using TheWrangler.GOAP.Targets;
using TheWrangler.GOAP.WorldKeys;
using UnityEngine;

namespace TheWrangler.GOAP.Factories
{
    [RequireComponent(typeof(DependencyInjector))]
    public class GoapSetConfigFactory : GoapSetFactoryBase
    {
        private DependencyInjector injector;

        public override IGoapSetConfig Create()
        {
            injector = GetComponent<DependencyInjector>();
            GoapSetBuilder builder = new("BaseSet");

            BuildGoals(builder);
            BuildActions(builder);
            BuildSensors(builder);

            return builder.Build();
        }

        private void BuildGoals(GoapSetBuilder builder)
        {
            builder.AddGoal<WanderGoal>()
                .AddCondition<IsWandering>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, 1);
            
            builder.AddGoal<KillTarget>()
                .AddCondition<TargetHealth>(CrashKonijn.Goap.Resolver.Comparison.SmallerThanOrEqual, 0);
        }

        private void BuildActions(GoapSetBuilder builder)
        {
            builder.AddAction<WanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWandering>(EffectType.Increase)
                .SetBaseCost(5)
                .SetInRange(10);

            builder.AddAction<MeleeAction>()
                .SetTarget<CharacterTarget>()
                .AddEffect<TargetHealth>(EffectType.Decrease)
                .SetBaseCost(injector.attackConfig.MeleeAttackCost)
                .SetInRange(injector.attackConfig.SensorRadius);
        }

        private void BuildSensors(GoapSetBuilder builder)
        {
            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();

            builder.AddTargetSensor<CharacterTargetSensor>()
                .SetTarget<CharacterTarget>();
        }
    }
}
