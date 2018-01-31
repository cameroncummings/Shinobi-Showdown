using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MiniMapControl : MonoBehaviour
{
    [SerializeField] private Renderer playerIcon;//The icon that your player always sees
    [SerializeField] private Renderer lanternPlayerIcon;// the icon that the opponent sometime sees

    private float defaultHeight = 20;//the default height the minimap camera follows the player at
    private float zoomedOutHeight = 30;//the height the minimap cam goes when it zooms out

    private float storedShadowDist;

    private void Update()
    {
        Vector3 pos = transform.position;
        //zooms out when the player hits the M key
        if (Input.GetKey(KeyCode.M))
        {
            pos.y = zoomedOutHeight;
        }
        else
        {
            pos.y = defaultHeight;
        }
        transform.position = pos;
        
        //tells it not to render the player icon for other players
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("MiniMapIcon"))
        {
            g.GetComponent<Renderer>().enabled = false;
        }
        playerIcon.enabled = true;
    }

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
