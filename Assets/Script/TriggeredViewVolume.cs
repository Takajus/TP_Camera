using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TriggeredViewVolume : AViewVolume
{
    [SerializeField]
    private GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        if (CheckTarget(other.gameObject))
        {
            SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckTarget(other.gameObject))
        {
            SetActive(false);
        }
    }

    private bool CheckTarget(GameObject obj)
    {
        return target == obj;
    }
   

    
}
