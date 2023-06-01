using Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AViewVolume : MonoBehaviour
{
   

    public int priority = 0;
    public AView view;

    private int uid;
    private static int nextUid = 0;

    public virtual float ComputeSelfWeight()
    {
        return 1.0f;
    }

    private void Awake()
    {
        uid = nextUid;
        nextUid++;
    }
    protected void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    protected bool IsActive { get; private set; }
    protected void SetAcctive(bool isActive)
    {
        if (isActive && !IsActive)
        {
            ViewVolumeBlender.Instance.AddVolume(this);
        }
        else if (!isActive && IsActive)
        {
            ViewVolumeBlender.Instance.RemoveVolume(this);
        }

        IsActive = isActive;
    }

}
