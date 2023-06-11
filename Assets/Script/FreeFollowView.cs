using System;
using UnityEngine;

namespace Script
{
    public class FreeFollowView : AView
    {
        [Header("Rotation")]
        [Range(0f, 360f)]
        public float yaw;
        [Range(-90f, 90f)]
        public float[] pitch = new float[3];
        [Range(-180f, 180f)]
        public float[] roll = new float[3];
        [Range(0f, 180f)]
        public float[] fieldOfView = new float[3];
        
        [Range(0f, 5f)]
        public float yawSpeed;

        [Header("Target")] 
        public Transform target;

        public Curve curve;
        public float curvePosition;
        [Range(0f, 10f)]
        public float curveSpeed;

        private Matrix4x4 tempMat;

        public override CameraConfiguration GetConfiguration()
        {
            CameraConfiguration configuration = new CameraConfiguration();

            Vector3 pos = target.position;
            
            float horizontal = Input.GetAxis("Horizontal");
            yaw += horizontal * yawSpeed * Time.deltaTime;
            configuration.yaw = yaw;

            float vertical = Input.GetAxis("Vertical");
            curvePosition += vertical * curveSpeed * Time.deltaTime;
            curvePosition = Mathf.Clamp01(curvePosition);

            Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
            Matrix4x4 curveToWorldMatrix = Matrix4x4.TRS(pos, rotation, Vector3.one);
            tempMat = curveToWorldMatrix;
            
            float tempPitch = 0f;
            float tempRoll = 0f; 
            float tempFov = 0f;

            if (curvePosition < 0.5f)
            {
                tempPitch = Mathf.Lerp(pitch[0], pitch[1], curvePosition * 2f);
                tempRoll = Mathf.Lerp(roll[0], roll[1], curvePosition * 2f);
                tempFov = Mathf.Lerp(fieldOfView[0], fieldOfView[1], curvePosition * 2f);
            }
            else
            {
                tempPitch = Mathf.Lerp(pitch[1], pitch[2], (curvePosition - 0.5f) * 2f);
                tempRoll = Mathf.Lerp(roll[1], roll[2], (curvePosition - 0.5f) * 2f);
                tempFov = Mathf.Lerp(fieldOfView[1], fieldOfView[2], (curvePosition - 0.5f) * 2f);
            }
            
            Quaternion rot = Quaternion.Euler(tempPitch, yaw, tempRoll);
            configuration.pivot = curve.GetPosition(curvePosition, curveToWorldMatrix);
            configuration.distanceAuPivot = 0f;
            configuration.pitch = tempPitch;
            configuration.roll = tempRoll;
            configuration.fieldOfView = tempFov;

            return configuration;
        }

        private void OnDrawGizmos()
        {
            curve.DrawGizmo(Color.yellow, tempMat, 20, curvePosition);
        }
    }
}
