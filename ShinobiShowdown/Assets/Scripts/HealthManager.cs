using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthManager : NetworkBehaviour
{
    public const int MAX_HEALTH = 3;//the max health that a player can have
    [SyncVar(hook = "OnChangeHealth")]private int m_health = MAX_HEALTH;//the current health of each player
    private Slider healthBar;//the slider which shows how many hearts you have
    [SerializeField] private GameObject kunaiModel;

    [SerializeField] private AudioSource m_DamageSFXSource;
    [SerializeField] private AudioClip damageSFX;
    [SerializeField] private AudioClip deathSFX;

    private bool startRespawnTimer;
    private float timer = 5;
    private GameObject progressBar;
    private GameObject respawnScreen;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        //finding the health bar
        progressBar = GameObject.FindGameObjectWithTag("RespawnProgressBar");
        respawnScreen = GameObject.FindGameObjectWithTag("RespawnScreen");

        progressBar.SetActive(false);
        respawnScreen.SetActive(false);

        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
    }

    private void Update()
    {
        if (startRespawnTimer && isLocalPlayer)
        {
            timer -= Time.deltaTime;
            progressBar.transform.GetChild(0).GetComponent<Image>().fillAmount = timer / 5;
            progressBar.transform.GetChild(2).GetComponent<Text>().text = ((int)timer + 1).ToString();
            if(timer <= 0)
            {
                timer = 5;
                startRespawnTimer = false;
                if (isServer)
                    RpcRespawn();
                else
                    CmdRespawn();
            }
        }
    }

    public void TakeDamage(int amount)
    {
        //only run on server
        if (!isServer)
        {
            return;
        }
        if (!m_DamageSFXSource.isPlaying)
        {
            m_DamageSFXSource.clip = damageSFX;
            m_DamageSFXSource.Play();
        }
        //subtract the specified amount
        m_health -= amount;
        
        //if the player runs out of health they die
        if (m_health <= 0)
        {
            m_health = 0;
            if (!m_DamageSFXSource.isPlaying)
            {
                m_DamageSFXSource.clip = deathSFX;
                m_DamageSFXSource.Play();
            }
            Debug.Log("Dead");
            if (isServer)
                RpcDeath();
            else
                CmdDeath();
        }
    }

    //changes the health UI for each individual player instead of just the host
    void OnChangeHealth(int currentHealth)
    {
        if(isLocalPlayer)
            healthBar.value = currentHealth;
    }

    [Command]
    void CmdDeath()
    {
        RpcDeath();
    }

    [Command]
    void CmdRespawn()
    {
        RpcRespawn();
    }

    [ClientRpc]
    void RpcDeath()
    {
        if (isLocalPlayer)
        {
            gameObject.GetComponent<NinjaController>().enabled = false;
            respawnScreen.SetActive(true);
            progressBar.SetActive(true);
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            kunaiModel.SetActive(false);
            startRespawnTimer = true;
        }
    }

    [ClientRpc]
    public void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            gameObject.GetComponent<NinjaController>().enabled = true;
            respawnScreen.SetActive(false);
            progressBar.SetActive(false);
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            kunaiModel.SetActive(true);
            GetComponent<KnifeManager>().CurrentAmmo = GetComponent<KnifeManager>().maxAmmo;
            GetComponent<NinjaController>().CurrentSmokeBombAmmo = GetComponent<NinjaController>().maxSmokeBombs;
            m_health = MAX_HEALTH;
            transform.position = GetComponent<NinjaController>().SpawnPosition;
        }
    }

}
