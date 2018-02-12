using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Knife : NetworkBehaviour
{
    private Text screenMessage;
    private bool hitSomething = false;
    public void Start()
    {
        screenMessage = GameObject.FindGameObjectWithTag("InputMessage").GetComponent<Text>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!hitSomething)
        {
            if (other.tag == "Lantern")
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            else if (other.tag == "Player")
            {
                other.GetComponent<PlayerHealth>().TakeDamage(1);
                Destroy(gameObject);
            }
            else
            {
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            hitSomething = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hitSomething)
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<SphereCollider>().enabled = true;

            if (other.tag == "Player")
            {
                screenMessage.enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (other.GetComponent<KnifeManager>().CurrentAmmo < other.GetComponent<KnifeManager>().maxAmmo)
                        other.GetComponent<KnifeManager>().CurrentAmmo++;
                    Destroy(gameObject);
                    screenMessage.enabled = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        screenMessage.enabled = false;
    }

}
