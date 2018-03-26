using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MiniMapControl : MonoBehaviour
{

    private float storedShadowDist;

    void OnPreRender()
    {
        //hiding the shadows and the fog
        storedShadowDist = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0;
        RenderSettings.fog = false;
        
    }

    void OnPostRender()
    {
        //resuming the shadows and fog for the other cameras
        QualitySettings.shadowDistance = storedShadowDist;
        RenderSettings.fog = true;
    }
}
