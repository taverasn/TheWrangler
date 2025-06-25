using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using TheWrangler.GOAP.Behaviour;
using TheWrangler.GOAP.Interfaces;
using UnityEngine;

namespace TheWrangler.GOAP.Actions
{
    public class ChargeAction : GoapActionBase<ChargeAction.Data>, IInjectable
    {
        private NeedsConfigSO needsConfig;
        private static readonly int IS_EATING = Animator.StringToHash("IsEating");

        public override void Created()
        {
        }

        public override void End(IMonoAgent agent, Data data)
        {
            //data.animator.SetBool(IS_EATING, false);
            data.charge.enabled = true;
        }

        public void Inject(DependencyInjector dependencyInjector)
        {
            needsConfig = dependencyInjector.needsConfig;
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.charge.enabled = false;
            data.Timer = needsConfig.NeedsCheckInterval;
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            data.Timer -= context.DeltaTime;
            data.charge.charge -= context.DeltaTime * needsConfig.NeedsRestorationRate;
            //data.animator.SetBool(IS_EATING, true);
            if (data.Target == null || data.charge.charge <= 0)
            {
                return ActionRunState.Stop;
            }

            return ActionRunState.Continue;
        }

        public class Data : CommonData
        {
            [GetComponent]
            public Animator animator { get; set; }

            [GetComponent]
            public ChargeBehaviour charge { get; set; }
        }
    }
}