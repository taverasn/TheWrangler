using CrashKonijn.Agent.Runtime;
using UnityEngine;

namespace TheWrangler.GOAP.Actions
{
    public class AttackData : CommonData
    {
        public static readonly int ATTACK = Animator.StringToHash("Attack");

        [GetComponent]
        public Animator animator { get; set; }
    }
}