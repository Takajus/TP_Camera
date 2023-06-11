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
        // Dessiner les sph�res du volume
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, outerRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
    private void Awake()
    {
        if (innerRadius > outerRadius)
        {
            (innerRadius, outerRadius) = (outerRadius, innerRadius);
        }
    }

    public void Update()
    {
        // Mettre � jour la distance entre la cible et cette vue
        if (target != null)
        {
            distance = Vector3.Distance(target.position, transform.position);

            // V�rifier si le volume doit �tre activ� ou d�sactiv� en fonction de la distance
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
        /*float targetDistance = Vector3.Distance(target.position,transform.position);

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

        return view.weight;*/
        float normalizedDistance = Mathf.Clamp01((distance - innerRadius) / (outerRadius - innerRadius));
        float weight = 1f - normalizedDistance;
        return weight;
    }
}
