using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class ThirdPersonCameraControl : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float sensitivityX = 2.0f;//the x sensitivity forr the mouse
        [SerializeField] private float sensitivityY = 1.0f;//the x sensitivity forr the mouse

        private const float Y_ANGLE_MIN = -25f;//the min value used for clamping the Y value 
        private const float Y_ANGLE_MAX = 25f;//the max value used for clamping the Y value 

        private float distance = 2.0f;//how far the camera is from the player
        private float currentX = 0.0f;
        private float currentY = 0.0f;



        private void Update()
        {

            if (player != null)
            {
                currentX += Input.GetAxis("Mouse X") * sensitivityX;
                currentX += Input.GetAxis("Right Stick X") * sensitivityX;
                currentX += Input.GetAxis("Horizontal") * sensitivityX;

                currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
                currentY -= Input.GetAxis("Right Stick Y") * sensitivityY;

                currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

                Transform camTransform = transform.parent.transform;
                camTransform.position = player.transform.position + new Vector3(0f, 1.2f, 0);
                camTransform.rotation = player.transform.rotation;
                //player.transform.rotation = Quaternion.Euler(currentY, currentX, 0);
                camTransform.rotation = Quaternion.Euler(currentY, currentX, 0);
            }

        }


    }
}
