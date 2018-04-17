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
            Physics.BoxCast(transform.position + new Vector3(0, 0.75f, 0), new Vector3(0.3f, .75f, 0.5f), transform.forward, out hit);
            distance = hit.distance;
            Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Player" && distance < 2)
            {
                CmdDamageTarget(hit.transform.gameObject, 2);
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

    [Command]
    void CmdDamageTarget(GameObject targetPlayer, float damage)
    {
        targetPlayer.GetComponent<HealthManager>().TakeDamage((int)damage);
    }

}
