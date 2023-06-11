using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    
    public class CameraController : MonoBehaviour
    {
        public Camera myCamera;
        public CameraConfiguration currentConfiguration = new CameraConfiguration();
        public CameraConfiguration targetConfiguration = new CameraConfiguration();
        [SerializeField]
        private float smoothness = 0.1f;
        [SerializeField]
        private CameraConfiguration _averageCameraConfiguration;
        private bool isCutRequested = false;

        private static CameraController _instance;
        public static CameraController Instance
        {
            get { return _instance; }
        }


        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        
        public void Cut()
        {
            isCutRequested = true;
        }

        public void Update()
        {
            targetConfiguration = InterpolateCameraController();
            
            if (isCutRequested)
            {
                
                currentConfiguration = targetConfiguration;
                isCutRequested = false;
                return;
            }
            else
            {
                if (smoothness * Time.deltaTime < 1)
                {
                    currentConfiguration = Smoothing(currentConfiguration, targetConfiguration, smoothness);
                }
                else
                {
                    currentConfiguration = targetConfiguration;
                }
            }
            
            ApplyConfiguration(myCamera, currentConfiguration);
        }

        // Application des paramètres de la class CameraConfiguration
        public void ApplyConfiguration(Camera cam, CameraConfiguration cameraConfiguration)
        {
            if (cam != null && cameraConfiguration != null)
            {
                cam.transform.rotation = cameraConfiguration.GetRotation();
                cam.transform.position = cameraConfiguration.GetPosition();
                cam.fieldOfView = cameraConfiguration.fieldOfView;
            }
        }

        public List<AView> _activeViews = new List<AView>();
        
        public void AddView(AView view)
        {
            if (!_activeViews.Contains(view))
            {
                _activeViews.Add(view);
            }
        }

        public void RemoveView(AView view)
        {
            if (_activeViews.Contains(view))
            {
                _activeViews.Remove(view);
            }
        }

        private CameraConfiguration InterpolateCameraController()
        {
            _averageCameraConfiguration = new CameraConfiguration();
            float totalWeight = 0f;
            Vector2 sum = Vector2.zero;
            
            foreach (AView activeView in _activeViews)
            {
                if(activeView.weight <= 0)
                    continue;
                
                CameraConfiguration configuration = activeView.GetConfiguration();
                sum += new Vector2(Mathf.Cos(configuration.yaw * Mathf.Deg2Rad), Mathf.Sin(configuration.yaw * Mathf.Deg2Rad)) * activeView.weight;
                _averageCameraConfiguration.pitch += configuration.pitch * activeView.weight;
                _averageCameraConfiguration.roll += configuration.roll * activeView.weight;
                _averageCameraConfiguration.pivot += configuration.pivot * activeView.weight;
                _averageCameraConfiguration.distanceAuPivot += configuration.distanceAuPivot * activeView.weight;
                _averageCameraConfiguration.fieldOfView += configuration.fieldOfView * activeView.weight;

                totalWeight += activeView.weight;
            }

            if (totalWeight <= 0)
            {
                Debug.LogError("Total weight is inferior or equal to ZERO !");
                return _averageCameraConfiguration;
            }

            _averageCameraConfiguration.yaw = Vector2.SignedAngle(Vector2.right, sum);
            _averageCameraConfiguration.pitch /= totalWeight;
            _averageCameraConfiguration.roll /= totalWeight;
            _averageCameraConfiguration.pivot /= totalWeight;
            _averageCameraConfiguration.distanceAuPivot /= totalWeight;
            _averageCameraConfiguration.fieldOfView /= totalWeight;

            return _averageCameraConfiguration;
        }
        
        private CameraConfiguration Smoothing(CameraConfiguration current, CameraConfiguration target, float speed)
        {
            current.yaw = Mathf.LerpAngle(current.yaw, target.yaw, speed);
            current.pitch = Mathf.Lerp(current.pitch, target.pitch, speed);
            current.roll = Mathf.Lerp(current.roll, target.roll, speed);
            current.pivot += (target.pivot - current.pivot) * speed * Time.deltaTime;
            current.distanceAuPivot = Mathf.Lerp(current.distanceAuPivot, target.distanceAuPivot, speed);
            current.fieldOfView += (target.fieldOfView - current.fieldOfView) * speed * Time.deltaTime;
            return current;
        }

        public Curve curve;
        [Range(0, 20)]
        public int sample = 20;
        [Range(0f, 1f)]
        public float pos;

        public void OnDrawGizmos()
        {
            foreach (AView view in _activeViews)
            {
                view.GetConfiguration().DrawGizmos(Color.red);
            }
            
            if(_averageCameraConfiguration != null)
                _averageCameraConfiguration.DrawGizmos(Color.blue);
            
            if(currentConfiguration != null)
                currentConfiguration.DrawGizmos(Color.green);

            if (curve != null)
            {
                curve.DrawGizmo(Color.yellow, transform.localToWorldMatrix, sample, pos);
            }
        }
        
    }
}
