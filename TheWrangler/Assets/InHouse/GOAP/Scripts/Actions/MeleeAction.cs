using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using TheWrangler.GOAP.Config;
using TheWrangler.GOAP.Interfaces;
using UnityEngine;

namespace TheWrangler.GOAP.Actions
{
    public class MeleeAction : GoapActionBase<AttackData>, IInjectable
    {
        private AttackConfigSO attackConfig;
        public override void Created()
        {
        }

        public override void End(IMonoAgent agent, AttackData data)
        {
            //data.animator.SetBool(AttackData.ATTACK, false);
        }

        public void Inject(DependencyInjector dependencyInjector)
        {
            attackConfig = dependencyInjector.attackConfig;
        }

        public override IActionRunState Perform(IMonoAgent agent, AttackData data, IActionContext context)
        {
            data.Timer -= context.DeltaTime;

            bool shouldAttack = data.Target != null &&
                                Vector3.Distance(data.Target.Position, agent.transform.position) <=
                                attackConfig.MeleeAttackRadius;

            //data.animator.SetBool(AttackData.ATTACK, shouldAttack);

            if (shouldAttack)
            {
                agent.transform.LookAt(data.Target.Position);
            }

            return data.Timer > 0 ? ActionRunState.Continue : ActionRunState.Stop;
        }

        public override void Start(IMonoAgent agent, AttackData data)
        {
            data.Timer = attackConfig.AttackDelay;
        }
    }
}