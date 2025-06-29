using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using System;
using UnityEngine;

namespace TheWrangler.GOAP
{
    [GoapId("Charge-70bd9651-decd-4ad1-8909-6626e73fefa6")]
    public class ChargeAction : GoapActionBase<ChargeAction.Data>
    {
        // This method is called when the action is created
        // This method is optional and can be removed

        bool actionComplete = false;

        public override void Created()
        {
            GameEventsManager.Instance.NeedsEvents.onBroadcastNeedsUpdate += OnBroadcastNeedsUpdate;
        }

        private void OnBroadcastNeedsUpdate(NeedsBroadcastEvent e)
        {
            if (e.owner == NeedsOwner.COMPANION && e.need.info.NeedsType == NeedsType.HUNGER && e.reason == NeedsBroadcastReason.REACHED_MAXIMUM)
            {
                actionComplete = true;
            }
        }

        // This method is called every frame before the action is performed
        // If this method returns false, the action will be stopped
        // This method is optional and can be removed
        public override bool IsValid(IActionReceiver agent, Data data)
        {
            return true;
        }

        // This method is called when the action is started
        // This method is optional and can be removed
        public override void Start(IMonoAgent agent, Data data)
        {
            GameEventsManager.Instance.NeedsEvents.UpdateNeeds(new NeedsUpdateEvent(NeedsOwner.COMPANION, "", NeedsType.HUNGER, NeedsUpdateReason.EFFECT_OVER_TIME_MAXIMUM, 0, 5));
        }

        // This method is called once before the action is performed
        // This method is optional and can be removed
        public override void BeforePerform(IMonoAgent agent, Data data)
        {
        }

        // This method is called every frame while the action is running
        // This method is required
        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (actionComplete)
                return ActionRunState.Completed;
            return ActionRunState.Continue;
        }

        // This method is called when the action is completed
        // This method is optional and can be removed
        public override void Complete(IMonoAgent agent, Data data)
        {
            actionComplete = false;
        }

        // This method is called when the action is stopped
        // This method is optional and can be removed
        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        // This method is called when the action is completed or stopped
        // This method is optional and can be removed
        public override void End(IMonoAgent agent, Data data)
        {
        }

        // The action class itself must be stateless!
        // All data should be stored in the data class
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
        }
    }
}