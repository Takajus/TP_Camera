using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class AviewVolume : MonoBehaviour
    {
        public int Priority = 0;
        public AView View;
        
        public bool isCutOnSwitch;
        
        internal int Uid;
        private static int NextUid = 0;

        public virtual float ComputeSelfWeight()
        {
            return 1.0f;
        }

        protected virtual void Awake()
        {
            Uid = NextUid;
            NextUid++;
        }
        
        protected bool IsActive { get; private set; }
        
        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                ViewvolumeBender.Instance.AddVolume(this);
            }
            else
            {
                ViewvolumeBender.Instance.RemoveVolume(this);
            }

            if (isCutOnSwitch)
            {
                CameraController.Instance.Update();
                CameraController.Instance.Cut();
            }

            IsActive = isActive;
        }
    }
    
}
