using Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class AViewVolume : MonoBehaviour
{
   

    public int priority = 0;
    public AView view;
    public bool isCutOnSwitch = true;
    public int Priority { get; set; } = 0;

    private int uid;
    private static int nextUid = 0;

    public virtual float ComputeSelfWeight()
    {
        return 1.0f;
        
    }
    public int Uid
    {
        get { return uid; }
    }
    private void Awake()
    {
        uid = nextUid;
        nextUid++;
    }
        


    protected bool IsActive { get; private set; }
    protected void SetActive(bool isActive)
    {
        if (isActive && !IsActive)
        {
            ViewVolumeBlender.Instance.AddVolume(this);
            if (isCutOnSwitch)
            {
                CameraController.Instance.Cut(); // Appeler Cut sur le CameraController
            }
        }
        else if (!isActive && IsActive)
        {
            ViewVolumeBlender.Instance.RemoveVolume(this);
            if (isCutOnSwitch)
            {
              
                CameraController.Instance.Cut(); // Appeler Cut sur le CameraController
            }
        }

        IsActive = isActive;
        

    }

}
