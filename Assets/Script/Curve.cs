using System;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Vector3 = UnityEngine.Vector3;

namespace Script
{
    [Serializable]
    public class Curve
    {
        public Vector3 A, B, C, D;

        public Vector3 GetPosition(float t)
        {
            return MathUtils.CubicBezier(A, B, C, D, t);
        }

        public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
        {
            Vector3 pWorld = localToWorldMatrix.MultiplyPoint(GetPosition(t));
            return pWorld;
        }

        public void DrawGizmo(Color c, Matrix4x4 localToWorldMatrix, int sample = 20, float pos = 0)
        {
            Gizmos.color = c;
            Gizmos.matrix = localToWorldMatrix;

            Gizmos.DrawSphere(A, 0.1f);
            Gizmos.DrawSphere(B, 0.1f);
            Gizmos.DrawSphere(C, 0.1f);
            Gizmos.DrawSphere(D, 0.1f);
            
            

            Vector3 prevPoint = GetPosition(0, localToWorldMatrix);
            for (int i = 0; i <= sample; i++)
            {
                float t = i / (float)sample;
                Vector3 point = GetPosition(t, localToWorldMatrix);
                Gizmos.DrawLine(prevPoint, point);
                prevPoint = point;
            }
            
            Gizmos.DrawSphere(GetPosition(pos, localToWorldMatrix), 0.15f);
            
            Gizmos.matrix = Matrix4x4.identity;
            
            
        }
    }
}
/*Vector3 aWorld = localToWorldMatrix.MultiplyPoint(A);
           Vector3 bWorld = localToWorldMatrix.MultiplyPoint(B);
           Vector3 cWorld = localToWorldMatrix.MultiplyPoint(C);
           Vector3 dWorld = localToWorldMatrix.MultiplyPoint(D);
           
           Gizmos.DrawLine(aWorld, bWorld);
           Gizmos.DrawLine(bWorld, cWorld);
           Gizmos.DrawLine(cWorld, dWorld);
           
           int sampleCount = sample;
           float step = 1f / sampleCount;

           for (int i = 0; i < sampleCount; i++)
           {
               float t = i * step;
               float nextT = (i + 1) * step;

               Vector3 p1 = GetPosition(t, localToWorldMatrix);
               Vector3 p2 = GetPosition(nextT, localToWorldMatrix);

               Gizmos.DrawLine(p1, p2);
           }

           Vector3 _pos = GetPosition(pos, localToWorldMatrix);
           
           Gizmos.DrawSphere(_pos, 0.25f);*/