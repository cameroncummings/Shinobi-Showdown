using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    [SerializeField] private GameObject indicatorRing;//The indicator ring that tells the player if they are going to step into the light
    private GameObject player;//the player that is the closest to the lantern used for checking the range for the indicator

    private const float INDICATOR_RANGE = 10;//the range at which the indicator starts to display itself

    private void Start()
    {
        
    }

    private void Update()
    {
        if (player == null)//checking that there is a player in the scene
            return;

        //finding the closest player
        foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Vector3.Distance(p.transform.position, indicatorRing.transform.position) < Vector3.Distance(player.transform.position, indicatorRing.transform.position))
            {
                player = p;
            }
        }

        //setting the alpha of the indicator ring based on how close the player is
        Color alpha = indicatorRing.GetComponent<SpriteRenderer>().color;
        alpha.a = 1.25f - Vector3.Distance(player.transform.position, indicatorRing.transform.position) / INDICATOR_RANGE;
        indicatorRing.GetComponent<SpriteRenderer>().color = alpha;
    }

    void OnTriggerStay(Collider other)
    {
        //broadcasts the players position by showing a indicator for 10 seconds
        if (other.tag == "Player")
        {
            StartCoroutine(showPlayer(other.GetComponentInChildren<SpriteRenderer>()));
        }
    }

    IEnumerator showPlayer(SpriteRenderer sprite)
    {

        sprite.enabled = true;//shows the idicator
        yield return new WaitForSeconds(1);//waits 10 seconds
        sprite.enabled = false;//hides the idicator
    }
}
