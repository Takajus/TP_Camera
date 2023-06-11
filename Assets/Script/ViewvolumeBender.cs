using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class ViewvolumeBender : MonoBehaviour
    {
        private static ViewvolumeBender instance;
        public static ViewvolumeBender Instance => instance ?? (instance = new ViewvolumeBender());

        private List<AviewVolume> ActiveViewVolumes = new List<AviewVolume>();
        private Dictionary<AView, List<AviewVolume>> VolumesPerViews = new Dictionary<AView, List<AviewVolume>>();

        public void AddVolume(AviewVolume volume)
        {
            ActiveViewVolumes.Add(volume);

            if (!VolumesPerViews.ContainsKey(volume.View))
            {
                VolumesPerViews[volume.View] = new List<AviewVolume>();
                volume.View.SetActive(true);
            }

            VolumesPerViews[volume.View].Add(volume);
        }

        public void RemoveVolume(AviewVolume volume)
        {
            ActiveViewVolumes.Remove(volume);

            if (VolumesPerViews.ContainsKey(volume.View))
            {
                VolumesPerViews[volume.View].Remove(volume);

                if (VolumesPerViews[volume.View].Count == 0)
                {
                    VolumesPerViews.Remove(volume.View);
                    volume.View.SetActive(false);
                }
            }
        }

        private void Update()
        {
            foreach (var aViewVolume in ActiveViewVolumes)
            {
                //volume.view.weight = 0f; // R�initialise le poids � 0
                aViewVolume.View.weight = 0f;
            }
        
            ActiveViewVolumes.Sort((volume1, volume2) =>
            {
                int priorityComparison = volume1.Priority.CompareTo(volume2.Priority);
                if (priorityComparison == 0)
                {
                    return volume1.Uid.CompareTo(volume2.Uid); // Trier par Uid croissant en cas d'�galit� de priorit�
                }
                return priorityComparison; // Trier par priorit� croissante
            });

            float totalWeight = 0.0f;

            foreach (AviewVolume volume in ActiveViewVolumes)
            {
            
                float weight = volume.ComputeSelfWeight();
                weight = Mathf.Clamp01(weight);

                float remainingWeight = 1.0f - weight;

                // Multiplier le poids de toutes les vues actives par remainingWeight
                foreach (AView view in VolumesPerViews.Keys)
                {
                    view.weight *= remainingWeight;
                }

                // Ajouter weight au poids de la vue associ�e au volume
                volume.View.weight += weight;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10,10,200,300));
            GUILayout.Label("Vue Actives : ");

            foreach (var volume in ViewvolumeBender.Instance.ActiveViewVolumes)
            {
                GUILayout.Label(volume.name);
            }
            
            GUILayout.EndArea();
        }
    }
    
}
