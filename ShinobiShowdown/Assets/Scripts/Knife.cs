using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lantern")
        {
            Destroy(this);
        }
        else if(other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(33.4f);
        }
        else
        {
            try
            {
                other.GetComponent<Renderer>().material.color = Color.red;
            }
            catch { }
            //this.transform.position -= this.transform.forward;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

}
