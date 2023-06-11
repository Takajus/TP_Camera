using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    
    public class CameraController : MonoBehaviour
    {
        public Camera myCamera;
        [SerializeField]
        private CameraConfiguration _averageCameraConfiguration;

        [SerializeField] private bool isCutRequested = false;

        private static CameraController _instance;
        public static CameraController Instance
        {
            get { return _instance; }
        }

        private List<AView> _activeViews = new List<AView>();

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

        private void Update()
        {
            if (isCutRequested)
            {
                _averageCameraConfiguration = InterpolateCameraController();
                ApplyConfiguration(myCamera, _averageCameraConfiguration);
                isCutRequested = false;
            }
            else
            {
                ApplyConfiguration(myCamera, InterpolateCameraController());
            }
        }

        // Application des paramètres de la class CameraConfiguration
        public void ApplyConfiguration(Camera cam, CameraConfiguration cameraConfiguration)
        {
            cam.transform.rotation = cameraConfiguration.GetRotation();
            cam.transform.position = cameraConfiguration.GetPosition();
            cam.fieldOfView = cameraConfiguration.fieldOfView;
        }

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
                //_averageCameraConfiguration.yaw += configuration.yaw * activeView.weight;
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
                return null;
            }

            _averageCameraConfiguration.yaw = Vector2.SignedAngle(Vector2.right, sum);
            _averageCameraConfiguration.pitch /= totalWeight;
            _averageCameraConfiguration.roll /= totalWeight;
            _averageCameraConfiguration.pivot /= totalWeight;
            _averageCameraConfiguration.distanceAuPivot /= totalWeight;
            _averageCameraConfiguration.fieldOfView /= totalWeight;

            return _averageCameraConfiguration;
        }
        public void Cut()
        {
            isCutRequested = true;
        }


        public void OnDrawGizmos()
        {
            foreach (AView view in _activeViews)
            {
                view.GetConfiguration().DrawGizmos(Color.red);
            }
            
            if(_averageCameraConfiguration != null)
                _averageCameraConfiguration.DrawGizmos(Color.blue);
        }
    }
}
