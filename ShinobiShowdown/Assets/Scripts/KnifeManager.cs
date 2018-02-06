using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeManager : MonoBehaviour
{
    [SerializeField] private GameObject Knife;
    [SerializeField] private float Knife_ThrowForce;

    public int maxAmmo;
    private int currentAmmo;
    public int CurrentAmmo { get { return currentAmmo; } set { currentAmmo = value; } }
    private Text currentKunaiCounter;

    private bool startTimer = false;
    private float timer = 0.0f;

    private void Start()
    {
        currentAmmo = maxAmmo;
        currentKunaiCounter = GameObject.FindGameObjectWithTag("KunaiCounter").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxisRaw("Fire1") != 0 || Input.GetAxisRaw("Right Trigger") != 0)  && !startTimer)
        {
            if(currentAmmo > 0)
            {
                GameObject tempKnifeHandler;
                tempKnifeHandler = Instantiate(Knife, Camera.main.transform.position + Camera.main.transform.forward * 3, Camera.main.transform.parent.rotation);
                tempKnifeHandler.transform.rotation = Quaternion.LookRotation(Camera.main.transform.right, Camera.main.transform.up);
                Rigidbody tempRigidBody = tempKnifeHandler.GetComponent<Rigidbody>();
                tempRigidBody.AddForce(Camera.main.transform.forward * 750);
                currentAmmo--;
                //NetworkServer.Spawn(tempKnifeHandler);
                Destroy(tempKnifeHandler, 5);
                startTimer = true;
            }
        }
        currentKunaiCounter.text = "Current Kunai Count: " + currentAmmo;
        if(startTimer)
        {
            if(timer >= 1)
            {
                startTimer = false;
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }
}
