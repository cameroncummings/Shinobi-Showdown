using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public void Start()
    {
        this.transform.rotation = new Quaternion(0, 90, 0, 1);
        this.GetComponent<Rigidbody>().freezeRotation = true ;
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
            other.GetComponent<PlayerHealth>().TakeDamage(33.4f);
        }
        else
        {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

}
