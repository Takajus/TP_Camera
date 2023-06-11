using Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    private List<AViewVolume> activeViewVolumes = new List<AViewVolume>();
    private Dictionary<AView, List<AViewVolume>> volumesPerViews = new Dictionary<AView, List<AViewVolume>>();

    private static ViewVolumeBlender instance;
    public static ViewVolumeBlender Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ViewVolumeBlender();
            }
            return instance;
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
    }

    public void AddVolume(AViewVolume volume)
    {
        activeViewVolumes.Add(volume);
        AView view = volume.view;

        if (!volumesPerViews.ContainsKey(view))
        {
            volumesPerViews[view] = new List<AViewVolume>();
            view.SetActive(true);
        }
        volumesPerViews[view].Add(volume);
    }


    public void RemoveVolume(AViewVolume volume) 
    {
        activeViewVolumes.Remove(volume);
        AView view = volume.view;

        if (volumesPerViews.ContainsKey(view))
        {
            volumesPerViews[view].Remove(volume);
            if (volumesPerViews[view].Count == 0)
            {
                volumesPerViews.Remove(view);
                view.SetActive(false);
            }
        }
    }
    public void Update()
    {
        foreach (var aViewVolume in activeViewVolumes)
        {
            //volume.view.weight = 0f; // R�initialise le poids � 0
            aViewVolume.view.weight = 0f;
        }
        
        activeViewVolumes.Sort((volume1, volume2) =>
        {
            int priorityComparison = volume1.Priority.CompareTo(volume2.Priority);
            if (priorityComparison == 0)
            {
                return volume1.Uid.CompareTo(volume2.Uid); // Trier par Uid croissant en cas d'�galit� de priorit�
            }
            return priorityComparison; // Trier par priorit� croissante
        });

        float totalWeight = 0.0f;

        foreach (AViewVolume volume in activeViewVolumes)
        {
            
            float weight = volume.ComputeSelfWeight();
            weight = Mathf.Clamp01(weight);

            float remainingWeight = 1.0f - weight;

            // Multiplier le poids de toutes les vues actives par remainingWeight
            foreach (AView view in volumesPerViews.Keys)
            {
                view.weight *= remainingWeight;
            }

            // Ajouter weight au poids de la vue associ�e au volume
            volume.view.weight += weight;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Active View Volumes:");
        
        foreach (AViewVolume volume in ViewVolumeBlender.Instance.activeViewVolumes)
        {
            GUILayout.Label(volume.name);
        }
    }
}
