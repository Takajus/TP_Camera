using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempMath : MonoBehaviour
{
    //public Vector3 a, b, c;
    public GameObject a, b, c;

    private void Start()
    {
        Vector3 AB = b.transform.position - a.transform.position;
        Debug.Log("AB " + AB);
        Debug.Log("magnitude " + AB.magnitude*AB.magnitude);
        Debug.Log("sqrMagnitude " + AB.sqrMagnitude);
        float distSqr = Vector3.Distance(a.transform.position, b.transform.position) *
                        Vector3.Distance(a.transform.position, b.transform.position);
        Debug.Log("Distance " + distSqr);
    }

    void Update()
    {
        Vector3 AB = b.transform.position - a.transform.position;
        Vector3 AC = c.transform.position - a.transform.position;
        float n = Vector3.Dot(AC, AB);
        n = n / AB.sqrMagnitude;
        n = Mathf.Clamp01(n);
        Vector3 t = a.transform.position + n * AB;
        Debug.Log(t);
    }
}
