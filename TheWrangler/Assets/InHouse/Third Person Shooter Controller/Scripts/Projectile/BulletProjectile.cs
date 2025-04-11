using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private int damage;
    private Rigidbody bulletRigidBody;
    [SerializeField] private float speed = 40f;
    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigidBody.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character_Base target = other.GetComponent<Character_Base>();
        if (target != null)
        {
            // Hit Target
            if (target.targetType != AITargetType.NPC_Friendly)
            {
                target.health.HealthChanged(-damage, AITargetType.Player);
                Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
            }
        }
        else
        {
            // Hit Something Else
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
