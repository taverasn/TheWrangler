using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using TheWrangler.GOAP.Config;
using TheWrangler.GOAP.Interfaces;
using UnityEngine;

namespace TheWrangler.GOAP.Actions
{
    public class WanderAction : GoapActionBase<CommonData>, IInjectable
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

        public override IActionRunState Perform(IMonoAgent agent, CommonData data, IActionContext context)
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
