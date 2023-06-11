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

        public bool isAuto;

        private float yaw, pitch;

        public override CameraConfiguration GetConfiguration()
        {
            CameraConfiguration configuration = new CameraConfiguration();
            

            Vector3 railPosition = new Vector3();
            
            if (!isAuto)
            {
                float horizontal = Input.GetAxis("Horizontal");
                distanceOnRail += horizontal * speed * Time.deltaTime;

                if (rail.isLoop)
                    distanceOnRail = Mathf.Repeat(distanceOnRail, rail.GetLength());
                else
                    distanceOnRail = Mathf.Clamp(distanceOnRail, 0f, rail.GetLength());
                
                Debug.Log(distanceOnRail);
                
                railPosition = rail.GetPosition(distanceOnRail);
            }
            else
            {
                railPosition = rail.GetPositionAuto(target);
            }
            
            Vector3 dir = (target.position - CameraController.Instance.myCamera.transform.position).normalized;
            //Vector3 dir = (target.position - railPosition).normalized;
            yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
            
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
