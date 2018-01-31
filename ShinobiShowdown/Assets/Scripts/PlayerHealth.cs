using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private float startingHealth = 3;
    [SerializeField] private float currentHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private AudioClip deathClip;

    private Animator anim;                                              // Reference to the Animator component.
    private AudioSource playerAudio;                                    // Reference to the AudioSource component.
    private bool isDead;                                                // Whether the player is dead.
    private bool damaged;                                               // True when the player gets damaged.
	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
        // Set the initial health of the player.
        currentHealth = startingHealth;
    }
	
	// Update is called once per frame

    public void TakeDamage(float amount)
    {
        if (!isLocalPlayer)
            return;
        damaged = true;
        currentHealth -= amount;
        //playerAudio.Play();
        healthBar.value = currentHealth;
        if (currentHealth <= 0 && !isDead)
        {
           // Death();
        }
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;
        // Tell the animator that the player is dead.
        anim.SetTrigger("Die");
        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        playerAudio.clip = deathClip;
        playerAudio.Play();
    }
}
