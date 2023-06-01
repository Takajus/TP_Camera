using Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    private static ViewVolumeBlender instance;
    private List<AViewVolume> activeViewVolumes;
    private Dictionary<AView, List<AViewVolume>> volumesPerViews;

    public static ViewVolumeBlender Instance
    {
        get
        {
            return instance;
        }
    }


    public void AddVolume(AViewVolume volume)
    {
        activeViewVolumes.Add(volume);

        if (!volumesPerViews.ContainsKey(volume.view))
        {
            volumesPerViews[volume.view] = new List<AViewVolume>();

            volume.view.gameObject.SetActive(true);
        }

        volumesPerViews[volume.view].Add(volume);
    }


    public void RemoveVolume(AViewVolume volume) 
    {
        activeViewVolumes.Remove(volume);
        volumesPerViews[volume.view].Remove(volume);

        if (volumesPerViews.ContainsKey(volume.view))
        {
            
            volumesPerViews[volume.view].Remove(volume);

            // Si la liste est vide après la suppression, supprime également la vue du dictionnaire
            if (volumesPerViews[volume.view].Count == 0)
            {
                volumesPerViews.Remove(volume.view);
                volume.view.gameObject.SetActive(false);
            }
        }


    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        activeViewVolumes = new List<AViewVolume>();
        volumesPerViews = new Dictionary<AView, List<AViewVolume>>();
    }

    public List<AViewVolume> GetActiveVolumes()
    {
        return activeViewVolumes;
    }

    private void OnGUI()
    {
        GUILayout.Label("Active View Volumes:");

        List<AViewVolume> activeVolumes = ViewVolumeBlender.Instance.GetActiveVolumes();

        foreach (AViewVolume volume in activeVolumes)
        {
            GUILayout.Label(volume.gameObject.name);
        }
    }
}
