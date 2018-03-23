﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class KnifeManager : NetworkBehaviour
{
    [SerializeField] private GameObject knifePrefab;//The prefab that holds the knife object
    [SerializeField] private GameObject knifeSpawnPos;//where the knife spawns from
    [SerializeField] private float throwForce;//how fast the knife gets thrown
    [SerializeField] private Camera m_Camera;//how fast the knife gets thrown

    private Text currentKunaiCounter;//a textbox that shows the current number of kunai
    private Text onScreenIndicatorMessage;

    public int maxAmmo;//how many kunais the player can carry

    [SyncVar(hook = "OnChangeAmmo")] private int m_CurrentAmmo;//holds the current number of ammo that a player has
    public int CurrentAmmo { get { return m_CurrentAmmo; } set { m_CurrentAmmo = value; } }//lets other classes access the current number of kunai

    private bool startTimer = false;//used to determine if a player just threw a kunai
    private float timer = 0;//holds how long the timer has been running
    private Animator m_Animator;

    private void Start()
    {
        //setting up some variables
        currentKunaiCounter = GameObject.FindGameObjectWithTag("KunaiCounter").GetComponent<Text>();
        m_CurrentAmmo = maxAmmo;
        m_Animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)//only runs if you are the player associated with this script
            return;

        if(Input.GetButton("Fire2") || Input.GetAxisRaw("Left Trigger") != 0)
        {
            Vector3 zoomedInPos = new Vector3(0,0,-0.5f);
            m_Camera.transform.localPosition = Vector3.Slerp(m_Camera.transform.localPosition, zoomedInPos, Time.deltaTime * 10);
        }
        else
        {
            Vector3 zoomedOutPos = new Vector3(0, 0, -2f);
            m_Camera.transform.localPosition = Vector3.Slerp(m_Camera.transform.localPosition, zoomedOutPos, Time.deltaTime * 10);
        }

        //throws a kunai knife when a player presses the left mouse button, or right trigger on a xbox controller
        if ((Input.GetButtonDown("Fire1") || Input.GetAxisRaw("Right Trigger") != 0) && !startTimer)
        {
            //calls a command on the server to deal with the kunais across clients
            StartCoroutine(ThrowAfterDelay(0.4f));
            startTimer = true;
        }

        //A delay of 0.5 seconds before a player can throw their next kunai
        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                timer = 0;
                startTimer = false;
            }
        }

        //updating the kunai counter text

    }

    public void showMessage(bool showMessage)
    {
        if (!isLocalPlayer)
            return;

        onScreenIndicatorMessage = GameObject.FindGameObjectWithTag("InputMessage").GetComponent<Text>();
        onScreenIndicatorMessage.enabled = showMessage;
    }

    IEnumerator ThrowAfterDelay(float delay)
    {
        if (m_CurrentAmmo > 0)
        {
            m_Animator.SetTrigger("ThrowKunai");
            yield return new WaitForSeconds(delay);
            CmdThrow(knifeSpawnPos.transform.position, knifeSpawnPos.transform.rotation);
        }
        yield return null;
    }

    [Command]
    void CmdThrow(Vector3 position, Quaternion rotation)
    {
        //creates a knife on the client and the server, as long as the player has enough ammo
        GameObject knife = Instantiate(knifePrefab, position, rotation);
        knife.transform.rotation = Quaternion.LookRotation(knife.transform.right, knife.transform.up);
        knife.GetComponent<Rigidbody>().velocity = -knife.transform.right * throwForce;
        m_CurrentAmmo--;
        NetworkServer.Spawn(knife.gameObject);
    }

    //changes the health UI for each individual player instead of just the host
    void OnChangeAmmo(int currentAmmo)
    {
        if (!isLocalPlayer)
            return;

        currentKunaiCounter.text = currentAmmo.ToString();
    }
}
