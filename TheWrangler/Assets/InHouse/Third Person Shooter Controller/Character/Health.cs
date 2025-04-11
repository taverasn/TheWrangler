using System;
using UnityEngine;

public class Health
{
    private int maxHealth;
    public int health { get; private set; }
    public bool isDead { get; private set; }

    public event Action<AITargetType> onDead;            
    public void Dead(AITargetType killedBy) => onDead?.Invoke(killedBy);

    public event Action<AITargetType> onDamageTaken; 
    public void DamageTaken(AITargetType damagedBy) => onDamageTaken?.Invoke(damagedBy);


    public Health(int _health)
    {
        maxHealth = _health;
        health = _health;
    }

    public void HealthChanged(int amount, AITargetType damagedBy)
    {
        if(isDead) return;
        health += amount;
        if(amount < 0)
        {
            DamageTaken(damagedBy);
        }
        if (health <= 0)
        {
            health = 0;
            isDead = true;
            Dead(damagedBy);
        } 
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Reset()
    {
        health = maxHealth;
        isDead = false;
    }
}
