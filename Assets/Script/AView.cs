using Script;
using UnityEngine;

namespace Script
{
    
    public abstract class AView : MonoBehaviour
    {
        public float weight;
        public bool isActiveOnStart;

        public virtual CameraConfiguration GetConfiguration()
        {
            return new CameraConfiguration();
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void Start()
        {
            if (isActiveOnStart)
            {
                SetActive(true);
                CameraController.Instance.AddView(this);
            }
            else
            {
                SetActive(false);
                CameraController.Instance.RemoveView(this);
            }
            
        }
    }
}
