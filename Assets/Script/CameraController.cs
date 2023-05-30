using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public CameraConfiguration configuration;
    
    private static CameraController instance;
    public static CameraController Instance { get; private set; }
 
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
 
        instance = this;
    }    
    
    // Application des paramètres de la class CameraConfiguration
    public void ApplyConfiguration(Camera camera, CameraConfiguration configuration)
    {
        camera.transform.rotation = configuration.GetRotation();
        camera.transform.position = configuration.GetPosition();
        camera.fieldOfView = configuration.fieldOfView;
    }
    
    
    
}
