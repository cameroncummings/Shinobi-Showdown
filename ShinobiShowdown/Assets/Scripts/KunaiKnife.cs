using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KunaiKnife : NetworkBehaviour
{
    //the message that displays when you are in range to pick up a knife
    private bool isMoving = true;//used to determine if it will damage the player or if they can pick it up


    private void OnTriggerEnter(Collider other)
    {
        if (isMoving)
        {
            //Destroys both the knife and the lantern when the knife collides with the lantern
            if (other.tag == "Lantern")
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }

            //If the knife connects with another player destroys the knife and that player takes 1 point of damage
            else if (other.tag == "Player")
            {
                other.GetComponent<HealthManager>().TakeDamage(1);
                Destroy(gameObject);
            }

            //If it collides with anything else stop movement
            else
            {
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            //isMoving = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (!isMoving)
        {
            //when the knife stops moving switches from the small box collider, to the larger sphere collider as the range to pick it up
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<SphereCollider>().enabled = true;

            //displays a message when the player enters the range to pick up the knife
            if (other.tag == "Player")
            {
                other.GetComponent<KnifeManager>().showMessage(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (other.GetComponent<KnifeManager>().CurrentAmmo < 7)
                        other.GetComponent<KnifeManager>().CurrentAmmo++;
                    Destroy(gameObject);
                    other.GetComponent<KnifeManager>().showMessage(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<KnifeManager>().showMessage(false);
    }

}
