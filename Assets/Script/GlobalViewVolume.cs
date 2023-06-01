using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalViewVolume : AViewVolume
{
    // Start is called before the first frame update
    void Start()
    {
        SetAcctive(true);
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
