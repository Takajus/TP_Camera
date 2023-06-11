using Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class AViewVolume : MonoBehaviour
{
    public int Priority = 0;
    public AView view;

    private int uid;
    private static int NextUid = 0;
    
    public bool isCutOnSwitch = false;
    
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
        uid = NextUid;
        NextUid++;
    }
    
    protected bool IsActive { get; private set; }
    protected void SetActive(bool isActive)
    {
        if (isActive)
        {
            ViewVolumeBlender.Instance.AddVolume(this);
            if (isCutOnSwitch)
            {
                ViewVolumeBlender.Instance.Update();
                CameraController.Instance.Cut(); // Appeler Cut sur le CameraController
            }
        }
        else
        {
            ViewVolumeBlender.Instance.RemoveVolume(this);
        }
        IsActive = isActive;
    }

}
