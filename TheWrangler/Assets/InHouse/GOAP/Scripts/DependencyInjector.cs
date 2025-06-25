using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using TheWrangler.GOAP.Config;
using TheWrangler.GOAP.Interfaces;

namespace TheWrangler.GOAP
{
    public class DependencyInjector : GoapConfigInitializerBase, IGoapInjector
    {
        public AttackConfigSO attackConfig;
        public WanderConfigSO wanderConfig;
        public NeedsConfigSO needsConfig;

        public override void InitConfig(IGoapConfig config)
        {
            config.GoapInjector = this;
        }

        public void Inject(ITargetSensor targetSensor)
        {
            if (targetSensor is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(IAction action)
        {
            if (action is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(IGoal goal)
        {
            if (goal is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(ISensor sensor)
        {
            if (sensor is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(IWorldSensor sensor)
        {
            if (sensor is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(ILocalWorldSensor sensor)
        {
            if (sensor is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(ILocalTargetSensor sensor)
        {
            if (sensor is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }
    }
}