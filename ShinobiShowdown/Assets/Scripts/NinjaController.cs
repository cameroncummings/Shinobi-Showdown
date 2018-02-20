using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class NinjaController : MonoBehaviour
{

    [SerializeField] float m_MovingTurnSpeed = 180;
    [SerializeField] float m_StationaryTurnSpeed = 90;
    [SerializeField] float m_RunCycleLegOffset = 0.2f;
    private const float Y_ANGLE_MIN = -25f;//the min value used for clamping the Y value 
    private const float Y_ANGLE_MAX = 25f;//the max value used for clamping the Y value 
    public GameObject mainCameraPrefab;
    private float m_ForwardAmount;
    private float m_TurnAmount;
    private float m_CurrentX;
    private float m_CurrentY;
    private Transform m_MainCam;
    private Vector3 m_Move;
    private Animator m_Animator;
    private GameObject mainCamera;

    // Use this for initialization
    void Start()
    {
        mainCamera = Instantiate(mainCameraPrefab, transform);
        m_Animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float v = CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        m_CurrentX += Input.GetAxis("Mouse X") * 4;
        m_CurrentX += Input.GetAxis("Right Stick X") * 4;
        m_CurrentX += Input.GetAxis("Horizontal");

        m_CurrentY -= Input.GetAxis("Mouse Y") * 1;
        m_CurrentY -= Input.GetAxis("Right Stick Y") * 1;

        m_CurrentY = Mathf.Clamp(m_CurrentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        Vector3 directionVector = v * mainCamera.transform.forward + h * mainCamera.transform.right;
        if (directionVector.magnitude > 1f) directionVector.Normalize();
        directionVector = transform.InverseTransformDirection(directionVector);

        directionVector = Vector3.ProjectOnPlane(directionVector, Vector3.up);

        m_TurnAmount = Mathf.Atan2(directionVector.x, directionVector.z);
        m_TurnAmount = Mathf.Clamp(m_TurnAmount, -1,1);


        if (Input.GetKey(KeyCode.W) || Input.GetAxisRaw("Vertical") > 0)
        {
            m_ForwardAmount = 0.5f;
            if( Input.GetKey(KeyCode.LeftShift)|| Input.GetButton("LeftStickPress"))
            {
                m_ForwardAmount = 1;
            }
        }
        else
        {
            m_ForwardAmount = 0;
        }


        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        //transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
        //m_TurnAmount = Mathf.Clamp(m_TurnAmount, -1, 1);

        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);

        //float runCycle = Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);

        transform.GetComponent<Rigidbody>().velocity = m_ForwardAmount * transform.forward * 20;

        mainCamera.transform.position = transform.position + new Vector3(0,1.2f,0);
        mainCamera.transform.rotation = transform.rotation;
        mainCamera.transform.rotation = Quaternion.Euler(m_CurrentY, m_CurrentX, 0);
    }
}
