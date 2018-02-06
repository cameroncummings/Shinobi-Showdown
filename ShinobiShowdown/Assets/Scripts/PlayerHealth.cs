using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private float maxHealth = 3;
    private float currentHealth;
    public float CurrentHealth{get{return currentHealth;}set{currentHealth = value;}}
    private List<GameObject> heartList = new List<GameObject>();
    //[SerializeField] private AudioClip deathClip;

    private Animator anim;                                              // Reference to the Animator component.
    private AudioSource playerAudio;                                    // Reference to the AudioSource component.
    private bool isDead;                                                // Whether the player is dead.                                           

	void Start ()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        //healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
        // Set the initial health of the player.
        currentHealth = maxHealth;
        heartList = new List<GameObject>(GameObject.FindGameObjectsWithTag("HeartUI"));
    }
	
	// Update is called once per frame

    public void TakeDamage(float amount)
    {
        if (!isLocalPlayer)
            return;
        currentHealth -= amount;
        if (heartList.Count > 0)
            heartList[0].SetActive(false);
            heartList.RemoveAt(0);
        //playerAudio.Play();
        //healthBar.value = health;
        if (currentHealth <= 0 && !isDead)
        {
            Debug.Log("Dead");
           // Destroy(gameObject);
            isDead = true;
           //Death();
        }
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        //isDead = true;
        // Tell the animator that the player is dead.
        //anim.SetTrigger("Die");
        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //playerAudio.clip = deathClip;
        //playerAudio.Play();
    }
}
