using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class KunaiKnife : NetworkBehaviour
{

    private NinjaController.Team kunaiTeam;
    public NinjaController.Team KunaiTeam { get { return kunaiTeam; } set { kunaiTeam = value; } }
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

                foreach (Renderer r in other.GetComponentsInChildren<Renderer>())
                {
                    r.material.SetColor("_EmissionColor", Color.black);
                }

                other.GetComponent<SphereCollider>().enabled = false;
                Destroy(gameObject);
                //CmdDestroyLight(other.gameObject);
            }
            else if (other.tag == "Player" && kunaiTeam != other.GetComponent<NinjaController>().m_playerTeam)
            {
                other.GetComponent<HealthManager>().TakeDamage(1);
                Destroy(gameObject);
            }
            else if (other.tag == "Target" && kunaiTeam == NinjaController.Team.ATTACKING)
            {
                SceneManager.LoadScene("OffenseWin");
            }
            else if (other.tag != "Player" && other.tag != "KunaiKnife")
            {
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                this.GetComponent<BoxCollider>().enabled = false;
                this.GetComponent<SphereCollider>().enabled = true;
                isMoving = false;
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                if (other.GetComponent<NinjaController>().CurrentAmmo < other.GetComponent<NinjaController>().maxKunais)
                    other.GetComponent<NinjaController>().CurrentAmmo++;
                Destroy(gameObject);
            }
        }
    }

    [Command]
    void CmdLoadEndScreen()
    {
        SceneManager.LoadScene("OffenseWin");
    }

}
