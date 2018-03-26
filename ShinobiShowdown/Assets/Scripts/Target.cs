using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "KunaiKnife")
        {
            Destroy(gameObject);
            Debug.Log("Killed Old Guy");
        }
    }
}
