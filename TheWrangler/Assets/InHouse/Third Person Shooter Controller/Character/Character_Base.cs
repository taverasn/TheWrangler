using UnityEngine;

public class Character_Base : MonoBehaviour
{
    [field: Header("Components")]
    [field: SerializeField] public AnimationHandler_Base animationHandler { get; private set; }
    [field:SerializeField] public AITargetType targetType { get; protected set; }
    [field: SerializeField] public int maxHealth { get; private set; } = 100;
    public Health health { get; private set; }

    public virtual void Awake()
    {
        health = new Health(maxHealth);
    }

    protected virtual void OnEnable()
    {
        health.onDamageTaken += OnDamageTaken;
        health.onDead += OnDead;
    }

    protected virtual void OnDisable()
    {
        health.onDamageTaken -= OnDamageTaken;
        health.onDead -= OnDead;
    }

    protected virtual void OnDamageTaken(AITargetType damagedBy)
    {
        animationHandler.TriggerHit();
    }

    protected virtual void OnDead(AITargetType killedBy)
    {
        HandleDeath(killedBy);
    }

    protected virtual void HandleDeath(AITargetType killedBy)
    {
        animationHandler.SetIsDead(true);
    }
}
