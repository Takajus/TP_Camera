using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

namespace Script
{
    public class DollyView : AView
    {
        [Header("Rotation")]
        [Range(-180f, 180f)]
        public float roll;
        
        [Range(0f, Single.PositiveInfinity)]
        public float distanceAuPivot;
        
        [Header("FoV")]
        [Range(0f, 180f)]
        public float fieldOfView;
        
        [Header("Target")]
        public Transform target;

        [Header("Rail")]
        public Rail rail;
        public float distanceOnRail;
        
        [Header("Speed")]
        public float speed;

        private float yaw, pitch;

        public override CameraConfiguration GetConfiguration()
        {
            CameraConfiguration configuration = new CameraConfiguration();
            
            float horizontalInput = Input.GetAxis("Horizontal");
            distanceOnRail += horizontalInput * speed * Time.deltaTime;
            
            if (rail.isLoop)
                distanceOnRail = Mathf.Repeat(distanceOnRail, rail.GetLength());
            else
                distanceOnRail = Mathf.Clamp(distanceOnRail, 0f, rail.GetLength());
            
            Debug.Log(distanceOnRail);

            Vector3 dir = (target.position - transform.position).normalized;
            yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
            
            Vector3 railPosition = rail.GetPosition(distanceOnRail);
            configuration.pivot = railPosition;
            configuration.distanceAuPivot = 0f;
            configuration.yaw = yaw;
            configuration.pitch = pitch;
            configuration.roll = roll;
            configuration.fieldOfView = fieldOfView;

            return configuration;
        }
    }
}
