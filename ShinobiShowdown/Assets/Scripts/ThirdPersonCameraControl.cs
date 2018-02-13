using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class ThirdPersonCameraControl : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float sensitivityX = 4.0f;//the x sensitivity forr the mouse
        [SerializeField] private float sensitivityY = 1f;//the x sensitivity forr the mouse
        private PauseMenu pause;
        private Transform playerPausePosition;

        private const float Y_ANGLE_MIN = -25f;//the min value used for clamping the Y value 
        private const float Y_ANGLE_MAX = 25f;//the max value used for clamping the Y value 

        private float distance = 2.0f;//how far the camera is from the player
        private float currentX = 0.0f;
        private float currentY = 0.0f;
        private float minDistance = 1;
        private float maxDistance = 2;
        Vector3 direction;

        private void Start()
        {
            pause = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PauseMenu>();
            direction = transform.localPosition.normalized;
            distance = transform.localPosition.magnitude;
        }


        private void Update()
        {
            float tempSpeed = player.GetComponent<Animator>().speed;
            player.GetComponent<ThirdPersonUserControl>().PauseGame(pause.isPaused);
            if (pause.isPaused)
            {
                player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;//| RigidbodyConstraints.FreezeRotation;
                player.GetComponent<Animator>().speed = 0;
                return;
            }
            else
            {
                player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                player.GetComponent<Animator>().speed = 1;
            }
            player.GetComponent<Animator>().speed = tempSpeed;
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentX += Input.GetAxis("Right Stick X") * sensitivityX;
            currentX += Input.GetAxis("Horizontal");

            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
            currentY -= Input.GetAxis("Right Stick Y") * sensitivityY;

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            transform.parent.transform.position = player.transform.position + new Vector3(0f, 1.2f, 0);
            transform.parent.transform.rotation = player.transform.rotation;
            transform.parent.transform.rotation = Quaternion.Euler(currentY, currentX, 0);

            Vector3 desiredCameraPos = transform.parent.TransformPoint(direction * maxDistance);
            RaycastHit hit;
            if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
            {
                distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
            else
            {
                distance = maxDistance;
            }
            transform.localPosition = Vector3.Slerp(transform.localPosition, direction * distance, Time.deltaTime * 10);
        }
    }
}
