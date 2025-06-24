using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using TheWrangler.GOAP.Config;
using TheWrangler.GOAP.Interfaces;
using UnityEngine;

namespace TheWrangler.GOAP.Actions
{
    public class WanderAction : ActionBase<CommonData>, IInjectable
    {
        private WanderConfigSO wanderConfig;
        public override void Created()
        {
        }

        public override void End(IMonoAgent agent, CommonData data)
        {
        }

        public void Inject(DependencyInjector dependencyInjector)
        {
            wanderConfig = dependencyInjector.wanderConfig;
        }

        public override ActionRunState Perform(IMonoAgent agent, CommonData data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;

            if (data.Timer > 0)
            {
                return ActionRunState.Continue;
            }
            return ActionRunState.Stop;
        }

        public override void Start(IMonoAgent agent, CommonData data)
        {
            data.Timer = Random.Range(wanderConfig.WaitRangeBetweenWanders.x, wanderConfig.WaitRangeBetweenWanders.y);
        }
    }
}
