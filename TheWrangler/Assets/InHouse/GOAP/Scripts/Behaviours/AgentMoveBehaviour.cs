using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace TheWrangler.GOAP.Behaviour
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(AgentBehaviour))]
    public class AgentMoveBehaviour : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator animator;
        private AgentBehaviour agentBehaviour;
        private ITarget currentTarget;
        [SerializeField]private float minMoveDistance = 0.25f;

        private Vector3 lastPosition;
        private static readonly int WALK = Animator.StringToHash("Walk");

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            agentBehaviour = agent.GetComponent<AgentBehaviour>();
        }

        private void OnEnable()
        {
            agentBehaviour.Events.OnTargetInRange += EventsOnTargetInRange;    
            agentBehaviour.Events.OnTargetChanged += EventsOnTargetChanged;    
            agentBehaviour.Events.OnTargetOutOfRange += EventsOnTargetOutOfRange;    
        }

        private void OnDisable()
        {
            agentBehaviour.Events.OnTargetInRange -= EventsOnTargetInRange;
            agentBehaviour.Events.OnTargetChanged -= EventsOnTargetChanged;
            agentBehaviour.Events.OnTargetOutOfRange -= EventsOnTargetOutOfRange;
        }

        private void EventsOnTargetInRange(ITarget target)
        {
        }

        private void EventsOnTargetChanged(ITarget target, bool inRange)
        {
            currentTarget = target;
            lastPosition = currentTarget.Position;
            agent.SetDestination(target.Position);
            //animator.SetBool(WALK, true);
        }

        private void EventsOnTargetOutOfRange(ITarget target)
        {
            //animator.SetBool(WALK, false);
        }

        private void Update()
        {
            if (currentTarget == null)
            {
                return;
            }

            if (minMoveDistance <= Vector3.Distance(currentTarget.Position, lastPosition))
            {
                lastPosition = currentTarget.Position;
                agent.SetDestination(currentTarget.Position);
            }
            //animator.SetBool(WALK, agent.velocity.magnitude > 0.1f);
        }
    }
}
