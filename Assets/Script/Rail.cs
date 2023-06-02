using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script
{
    public class Rail : MonoBehaviour
    {
        [SerializeField] private List<Transform> followPath = new List<Transform>();
        private List<float> segmentLengths = new List<float>();
        public bool isLoop;

        private float length;

        void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                followPath.Add(transform.GetChild(i));

                if (i > 0)
                {
                    Vector3 previousNodePosition = followPath[i - 1].position;
                    Vector3 currentNodePosition = followPath[i].position;
                    float segmentLength = Vector3.Distance(previousNodePosition, currentNodePosition);
                    segmentLengths.Add(segmentLength);
                    length += segmentLength;
                }
                else
                    segmentLengths.Add(0f);
            }
            
            Debug.Log(length);

            if (isLoop)
            {
                var temp = Vector3.Distance(followPath[followPath.Count - 1].position, followPath[0].position);
                length += temp;
                segmentLengths.Add(temp);
            }
        }

        public float GetLength()
        {
            return length;
        }

        public Vector3 GetPosition(float distance)
        {
            if (distance <= 0.01f)
            {
                return followPath[0].position;
            }
            else if (distance >= length)
            {
                if (isLoop)
                {
                    return followPath[0].position;
                }
                else
                {
                    return followPath[followPath.Capacity - 1].position;
                }
            }
            else
            {
                int startIndex = 0;
                int endIndex = 0;
                endIndex = followPath.Count - 1;

                for (int i = 0; i < segmentLengths.Count; i++)
                {
                    if (distance <= segmentLengths[i])
                    {
                        endIndex = i;
                        break;
                    }
                    else
                    {
                        distance -= segmentLengths[i];
                        startIndex++;
                    }
                }
                    
                float t = distance / segmentLengths[endIndex];
                
                if(!isLoop)
                    return Vector3.Lerp(followPath[startIndex - 1].position, followPath[endIndex].position, t);
                else
                {
                    if (endIndex >= followPath.Count)
                        return Vector3.Lerp(followPath[endIndex - 1].position, followPath[0].position, t);
                    
                    return Vector3.Lerp(followPath[startIndex - 1].position, followPath[endIndex].position, t);

                }
            }
        }

        private void OnDrawGizmos()
        {
            if (followPath == null || followPath.Count < 2)
                return;

            for (int i = 0; i < followPath.Count; i++)
            {
                Gizmos.color = Color.black;
                Vector3 currentNodePosition = followPath[i].position;

                if (isLoop)
                {
                    Vector3 nextNodePosition = followPath[(i + 1) % followPath.Count].position;
                    Gizmos.DrawLine(currentNodePosition, nextNodePosition);
                }
                else
                {
                    if (i < followPath.Count - 1)
                    {
                        Vector3 nextNodePosition = followPath[i + 1].position;
                        Gizmos.DrawLine(currentNodePosition, nextNodePosition);
                    }
                }

                Gizmos.color = Color.gray;
                Gizmos.DrawSphere(currentNodePosition, 0.25f);
            }
        }
    }
}
