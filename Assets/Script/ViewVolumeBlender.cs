using Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    private static ViewVolumeBlender instance;
    private List<AViewVolume> activeViewVolumes;
    private Dictionary<AView, List<AViewVolume>> volumesPerViews;
    private List<AView> activeViews;

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
            activeViews.Add(volume.view);
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
                activeViews.Remove(volume.view);
            }

        }


    }
    private void Update()
    {
        
        activeViewVolumes.Sort((volume1, volume2) =>
        {
            if (volume1.Priority == volume2.Priority)
            {
                return volume1.Uid.CompareTo(volume2.Uid); // Trier par Uid croissant en cas d'égalité de priorité
            }
            return volume1.Priority.CompareTo(volume2.Priority); // Trier par priorité croissante
        });

        foreach (AViewVolume volume in activeViewVolumes)
        {
            volume.view.weight = 0f; // Réinitialise le poids à 0
            float weight = volume.ComputeSelfWeight();
            weight = Mathf.Clamp01(weight);

            float remainingWeight = 1.0f - weight;

            // Multiplier le poids de toutes les vues actives par remainingWeight
            foreach (AView view in activeViews)
            {
                view.weight *= remainingWeight;
            }

            // Ajouter weight au poids de la vue associée au volume
            volume.view.weight += weight;
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
        activeViews = new List<AView>();
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
