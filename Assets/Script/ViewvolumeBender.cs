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
        
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10,10,200,300));
            GUILayout.Label("Vue Actives : ");

            foreach (var volume in ActiveViewVolumes)
            {
                GUILayout.Label(volume.gameObject.name);
            }
            
            GUILayout.EndArea();
        }
    }
    
}
