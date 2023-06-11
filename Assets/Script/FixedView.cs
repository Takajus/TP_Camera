using System;
using UnityEngine;

namespace Script
{
    
    public class FixedView : AView
    {
        [Header("Rotation")]
        [Range(0f, 360f)]
        public float yaw;
        [Range(-90f, 90f)]
        public float pitch;
        [Range(-180f, 180f)]
        public float roll;

        [Header("FoV")]
        [Range(0f, 180f)]
        public float fieldOfView;

        public override CameraConfiguration GetConfiguration()
        {
            CameraConfiguration configuration = new CameraConfiguration();
            configuration.pivot = transform.position;
            configuration.distanceAuPivot = 0f;
            configuration.yaw = yaw;
            configuration.pitch = pitch;
            configuration.roll = roll;
            configuration.fieldOfView = fieldOfView;

            return configuration;
        }
        
    }
}
