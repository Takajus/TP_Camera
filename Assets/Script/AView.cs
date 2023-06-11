using Script;
using UnityEngine;

namespace Script
{
    
    public abstract class AView : MonoBehaviour
    {
        public float weight;
       
        public virtual CameraConfiguration GetConfiguration()
        {
            return new CameraConfiguration();
        }

        public void SetActive(bool isActive)
        {
            if (isActive) 
            { 
                CameraController.Instance.AddView(this); 
            } 
            else 
            { 
                CameraController.Instance.RemoveView(this); 
            } 
        }
        
        private void OnDrawGizmos()
        {
            GetConfiguration().DrawGizmos(Color.gray);
        }

    }
}
