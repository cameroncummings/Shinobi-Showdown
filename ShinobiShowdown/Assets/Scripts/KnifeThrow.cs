using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeThrow : MonoBehaviour
{

    [SerializeField] private GameObject KnifeSpawnPoint;
    [SerializeField] private GameObject Knife;
    [SerializeField] private float Knife_ThrowForce;

    private bool startTimer = false;
    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Fire1") != 0  && !startTimer)
        {

            GameObject tempKnifeHandler;
            tempKnifeHandler = Instantiate(Knife, KnifeSpawnPoint.transform.position, KnifeSpawnPoint.transform.rotation);
            //tempKnifeHandler.transform.Rotate(Vector3.left * 90);
            Rigidbody tempRigidBody = tempKnifeHandler.GetComponent<Rigidbody>();
            tempRigidBody.AddForce(transform.forward * Knife_ThrowForce);
            startTimer = true;
        }
        if(startTimer)
        {
            if(timer >= 2)
            {
                startTimer = false;
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }
}
