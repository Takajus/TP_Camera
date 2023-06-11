using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class TriggerViewVolume : AviewVolume
{
    public GameObject target;
    
    private void OnTriggerEnter(Collider other)
    {
        if (target != null && other.gameObject == target)
        {
            SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target != null && other.gameObject == target)
        {
            SetActive(false);
        }
    }
}
