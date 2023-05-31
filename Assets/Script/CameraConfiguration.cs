using System;
using UnityEngine;

namespace Script
{
    [Serializable]
    public class CameraConfiguration
    {
        [Header("Rotation")]
        [Range(0f, 360f)]
        public float yaw;
        [Range(-90f, 90f)]
        public float pitch;
        [Range(-180f, 180f)]
        public float roll;
        
        [Header("Position")]
        public Vector3 pivot;
        [Range(0f, Single.PositiveInfinity)]
        public float distanceAuPivot;
        
        [Header("FoV")]
        [Range(0f, 180f)]
        public float fieldOfView;

        public Quaternion GetRotation()
        {
            return Quaternion.Euler(pitch, yaw, roll);
        }

        public Vector3 GetPosition()
        {
            Vector3 offset = GetRotation() * (new Vector3(0,0,-1) * distanceAuPivot);
            return pivot + offset;
        }
        
        public void DrawGizmos(Color color)
        {
            Gizmos.color = color;
            //Gizmos.DrawSphere(pivot, 0.25f);
            Gizmos.DrawCube(pivot, new Vector3(0.4f, 0.4f, 0.75f));
            Vector3 position = GetPosition();
            Gizmos.DrawLine(pivot, position);
            Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
            if (Camera.main != null) Gizmos.DrawFrustum(Vector3.zero, fieldOfView, 0.5f, 0f, Camera.main.aspect);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
    
}
