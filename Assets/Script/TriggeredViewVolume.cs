using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TriggeredViewVolume : AViewVolume
{
    public GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        if (CheckTarget(other))
        {
            SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckTarget(other))
        {
            SetActive(false);
        }
    }

    private bool CheckTarget(Collider obj)
    {
        return obj.gameObject == target;
    }

}
