using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectileRaycast : MonoBehaviour
{
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Vector3 targetPosition;
    private Transform currentHitFX;

    public void SetUp(Vector3 _targetPosition, bool hit)
    {
        targetPosition = _targetPosition;
        if (hit)
        {
            currentHitFX = vfxHitGreen;
        }
        else
        {
            currentHitFX = vfxHitRed;
        }
    }

    private void Update()
    {
        float distanceBefore = Vector3.Distance(transform.position, targetPosition);

        Vector3 movDir = (targetPosition - transform.position).normalized;
        float moveSpeed = 200f;
        transform.position += movDir * moveSpeed * Time.deltaTime;

        float distanceAfter = Vector3.Distance(transform.position, targetPosition);

        if(distanceBefore < distanceAfter)
        {
            Instantiate(currentHitFX, targetPosition, Quaternion.identity);
            transform.Find("Trail").SetParent(null);
            Destroy(gameObject);
        }
    }
}
