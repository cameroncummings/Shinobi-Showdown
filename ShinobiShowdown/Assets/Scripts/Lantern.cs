using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    private GameObject player;//the player that is the closest to the lantern used for checking the range for the indicator
    private const float INDICATOR_RANGE = 10;//the range at which the indicator starts to display itself

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(HidePlayer(other.GetComponentInChildren<SpriteRenderer>()));
            //other.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }

    IEnumerator HidePlayer(SpriteRenderer sprite)
    {
        yield return new WaitForSeconds(3.0f);//waits 10 seconds
        sprite.enabled = false;//hides the idicator
    }
}
