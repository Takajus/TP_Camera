using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

namespace Script
{
    public class FixedFollowView : AView
    {
        [Header("Rotation")]
        [Range(-180f, 180f)]
        public float roll;
        
        [Header("FoV")]
        [Range(0f, 180f)]
        public float fieldOfView;
        
        [Header("Target")]
        public GameObject target;

        public GameObject centralPoint;
        public float yawOffsetMax;
        public float pitchOffsetMax;

        public override CameraConfiguration GetConfiguration()
        {
            CameraConfiguration config = new CameraConfiguration();
            Vector3 dir = (target.transform.position - transform.position).normalized;
            float yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
            
            float centralYaw = Mathf.Atan2(centralPoint.transform.forward.x, centralPoint.transform.forward.z) * Mathf.Rad2Deg;
            if (Mathf.Abs(centralYaw - yaw) > 180f)
            {
                if (centralYaw < yaw)
                    centralYaw += 360f;
                else
                    yaw += 360f;
            }
            float deltaYaw = Mathf.Clamp(yaw - centralYaw, -yawOffsetMax, yawOffsetMax);
            config.yaw = centralYaw + deltaYaw;

            float centralPitch = -Mathf.Asin(centralPoint.transform.forward.y) * Mathf.Rad2Deg;
            float deltaPitch = Mathf.Clamp(pitch - centralPitch, -pitchOffsetMax, pitchOffsetMax);
            config.pitch = centralPitch + deltaPitch;
            
            config.pivot = transform.position;
            config.distanceAuPivot = 0f;
            config.roll = roll;
            config.fieldOfView = fieldOfView;
            
            return config;
        }
    }
    
}
