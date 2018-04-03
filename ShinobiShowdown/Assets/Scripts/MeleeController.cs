using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MeleeController : NetworkBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private AudioSource m_ThrowSFXSource;
    [SerializeField] private AudioClip stabSFX;

    private bool startTimer;
    private float timer;

    float distance;

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetButtonDown("RightStickPress") && !startTimer)
        {
            m_Animator.SetTrigger("StabKunai");
            m_ThrowSFXSource.clip = stabSFX;
            m_ThrowSFXSource.Play();
            RaycastHit hit;
            Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z) + transform.forward / 10, transform.TransformDirection(Vector3.forward), out hit);
            distance = hit.distance;
            if (hit.transform.tag == "Player" && distance < 2)
            {
                hit.transform.GetComponent<HealthManager>().TakeDamage(2);
            }
            startTimer = true;
        }

        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer > 0.6f)
            {
                timer = 0;
                startTimer = false;
            }
        }
    }

}
