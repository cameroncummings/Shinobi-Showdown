using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    public const int MAX_HEALTH = 3;
    [SyncVar(hook = "OnChangeHealth")]private int m_health = MAX_HEALTH;
    private Slider healthBar;

    private void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }

        m_health -= amount;
        

        if (m_health <= 0)
        {
            m_health = 0;
            Debug.Log("Dead");
            Destroy(gameObject);
        }
    }

    void OnChangeHealth(int currentHealth)
    {
        if(isLocalPlayer)
            healthBar.value = currentHealth;
    }

}
