using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonCameraControl : MonoBehaviour
{
    [SerializeField] private float sensitivityX = 2.0f;//the x sensitivity forr the mouse
    [SerializeField] private float sensitivityY = 1.0f;//the x sensitivity forr the mouse
    [SerializeField] private Transform lookAt;//the point at which the camera is looking at

    private const float Y_ANGLE_MIN = -25f;//the min value used for clamping the Y value 
    private const float Y_ANGLE_MAX = 25f;//the max value used for clamping the Y value 

    private float distance = 2.0f;//how far the camera is from the player
    private float currentX = 0.0f;
    private float currentY = 0.0f;



    private void Update()
    {
        //calculating the mouse input
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentX += Input.GetAxis("Right Stick X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
        currentY -= Input.GetAxis("Right Stick Y") * sensitivityY;

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        //TODO: Pause Menu

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //telling the camera to look at the player and rotating the camera to match the mouse movement
        lookAt.rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.LookAt(lookAt.position);
    }
}
