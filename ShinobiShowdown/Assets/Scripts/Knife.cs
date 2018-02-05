using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Knife : NetworkBehaviour
{
    public void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lantern")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(50);
        }
        else
        {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

}
