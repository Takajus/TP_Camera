using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    public Transform target;
    public float outerRadius;
    public float innerRadius;

    private float distance;

    private void OnDrawGizmos()
    {
        // Dessiner les sphères du volume
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, outerRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
    private void Awake()
    {
        if (innerRadius > outerRadius)
        {
            float temp = innerRadius;
            innerRadius = outerRadius;
            outerRadius = temp;
        }
    }

    public void Update()
    {
        // Mettre à jour la distance entre la cible et cette vue
        if (target != null)
        {
            distance = Vector3.Distance(target.position, transform.position);

            // Vérifier si le volume doit être activé ou désactivé en fonction de la distance
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
      

                float targetDistance = Vector3.Distance(target.position,transform.position);

                if (targetDistance >= outerRadius)
                {
                    view.weight = 0.0f;
                }
                else if (targetDistance < innerRadius)
                {
                    view.weight = 1.0f;
                }
                else
                {
                    view.weight = 1.0f - ((targetDistance - innerRadius) / (outerRadius - innerRadius));
                }
                

        view.weight = Mathf.Clamp01(view.weight);

        return view.weight;
      
    }


}
