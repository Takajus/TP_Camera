using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class SphereviewVolume : AviewVolume


{
    public Transform target;
    public float outerRadius;
    public float innerRadius;

    private float distance;

    private void Update()
    {

        if (target != null)
        {
            distance = Vector3.Distance(target.position, transform.position);

            if (distance <= outerRadius && !IsActive)
            {
                SetActive(true);
            }
            else if (distance > outerRadius && IsActive)
            {
                SetActive(false);
            }
        }
    }

    public override float ComputeSelfWeight()
    {
        float weight = 0f;

        if (outerRadius > innerRadius)
        {
            float normalizedDistance = Mathf.Clamp01((distance - innerRadius) / (outerRadius - innerRadius));
            weight = 1f - normalizedDistance;
        }

        return weight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, outerRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
}
