using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class KunaiKnife : NetworkBehaviour
{
    //the message that displays when you are in range to pick up a knife
    private bool isMoving = true;//used to determine if it will damage the player or if they can pick it up

    private void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;

        float emission = Mathf.PingPong(Time.time, 1.0f);
        Color baseColor = Color.grey; //Replace this with whatever you want for your base color at emission level '1'

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isMoving)
        {
            if (other.tag == "Lantern")
            {
                other.GetComponentInChildren<Light>().enabled = false;
                other.GetComponent<Lantern>().enabled = false;
                other.GetComponentInChildren<SpriteRenderer>().enabled = false;
                Destroy(gameObject);
            }
            else if (other.tag == "Player")
            {
                other.GetComponent<HealthManager>().TakeDamage(1);
                Destroy(gameObject);
            }
            else
            {
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            isMoving = false;
        }
        else
        {
            if (other.tag == "Player")
            {
                if (other.GetComponent<KnifeManager>().CurrentAmmo < other.GetComponent<KnifeManager>().maxAmmo)
                    other.GetComponent<KnifeManager>().CurrentAmmo++;
                Destroy(gameObject);
            }
        }
    }

}
