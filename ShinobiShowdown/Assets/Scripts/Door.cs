using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform openedPosition;
    public Transform closedPosition;
    private bool isOpened = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetButtonDown("Interact"))
            {
                other.GetComponent<Animator>().SetTrigger("DoorOpen");
                ToggleDoor();//other.GetComponent<Animator>().SetTrigger("DoorClose");
            }
        }
    }

    public void ToggleDoor()
    {
        if (isOpened)
        {
            //playerAnimator.GetComponent<Animation>()["Door Open"].speed = 1;
            //playerAnimator.GetComponent<Animation>()["Door Open"].time = playerAnimator.GetComponent<Animation>()["Door Open"].length;
            StartCoroutine(CloseDoor());
        }
        else
        {
            //    playerAnimator.GetComponent<Animation>()["Door Open"].speed = -1;
            //    playerAnimator.GetComponent<Animation>()["Door Open"].time = playerAnimator.GetComponent<Animation>()["Door Open"].length;
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        while (!isOpened)
        {
            transform.position = Vector3.MoveTowards(transform.position, openedPosition.position, Time.deltaTime * 2);
            if (transform.position == openedPosition.position)
            {
                isOpened = true;
            }
            yield return null;
        }
    }

    IEnumerator CloseDoor()
    {
        while (isOpened)
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPosition.position, Time.deltaTime * 2);
            if (transform.position == closedPosition.position)
            {
                isOpened = false;
            }
            yield return null;
        }
    }

}
