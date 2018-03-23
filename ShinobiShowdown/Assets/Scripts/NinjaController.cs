using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class NinjaController : NetworkBehaviour
{

    [SerializeField] private float stationaryTurnSpeed;
    [SerializeField] private float movingTurnSpeed;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float cameraMaxAngleY;//the min value used for clamping the Y value 
    [SerializeField] private float cameraMinAngleY;//the max value used for clamping the Y value 

    [SerializeField] private float mouseSensitivityX;//the min value used for clamping the Y value 
    [SerializeField] private float mouseSensitivityY;//the max value used for clamping the Y value 

    private Animator m_Animator;//holds the characters animation controller  
    private Rigidbody m_RigidBody;//holds the characters rigidbody

    [SerializeField] private Transform mainCamera;//holds the camera object inside the player object
    [SerializeField] private Camera miniMapCamera;
    [SerializeField] private GameObject smokeBombPrefab;
    [SerializeField] private float throwForce;
    [SerializeField] private Transform smokeBombSpawnPOS;
    public int maxSmokeBombs;

    private float m_ForwardAmount;//how much the player is trying to move forward in a frame
    private float m_TurnAmount;//how much the player is trying to turn in a frame

    private float m_CurrentX = 0;//holds the mouse movement in the x-Axis
    private float m_CurrentY = 0;//holds the mouse movement in the y-Axis

    Vector3 direction;//the direction the camera is facing
    float distance;//the distance from the camera to the player

    private float minDistance = 0.5f;//how close the camera can get to the player
    private float maxDistance = 2;//how far the camera can get to the player

    private Vector3 originalSpawnPos;
    public Vector3 SpawnPosition { get { return originalSpawnPos; } set { originalSpawnPos = value; } }

    private GameObject m_UIElements;
    private bool isPaused = false;
    private bool inRangeOfDoor = false;
    private bool startTimer = false;
    private float timer = 0;
    [SyncVar(hook = "OnChangeAmmo")] private int m_CurrentSmokeBombs;
    public int CurrentSmokeBombAmmo { get { return m_CurrentSmokeBombs; } set { m_CurrentSmokeBombs = value; } }
    private Text currentSmokeBombUICounter;

    void Start()
    {
        if (!isLocalPlayer)
        {
            mainCamera.GetComponent<Camera>().enabled = false;
            mainCamera.GetComponent<AudioListener>().enabled = false;
            miniMapCamera.enabled = false;
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        //setting up some variables 
        m_CurrentSmokeBombs = maxSmokeBombs;
        m_UIElements = GameObject.FindGameObjectWithTag("UIElements");
        m_Animator = gameObject.GetComponent<Animator>();
        m_RigidBody = gameObject.GetComponent<Rigidbody>();
        direction = mainCamera.localPosition.normalized;
        distance = mainCamera.localPosition.magnitude;
        originalSpawnPos = transform.position;
    }

    private void Update()
    {

        if (!isLocalPlayer)
            return;
        RaycastHit hit;
        if (Input.GetButtonDown("RightStickPress"))
        {
            m_Animator.SetTrigger("StabKunai");
            if (Physics.Linecast(new Vector3(transform.position.x, 1.3f, transform.position.z), transform.forward * 3, out hit))
            {
                if (hit.collider == gameObject.GetComponent<Collider>())
                {
                    hit.transform.GetComponent<HealthManager>().TakeDamage(1);
                }
            }
        }
        if (Input.GetButtonUp("Right Bumper") && !startTimer)
        {
            StartCoroutine(ThrowAfterDelay(0.4f));
            startTimer = true;
        }

        //A delay of 0.5 seconds before a player can throw their next smoke bomb
        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                timer = 0;
                startTimer = false;
            }
        }
    }

    IEnumerator ThrowAfterDelay(float delay)
    {
        if (m_CurrentSmokeBombs > 0)
        {
            m_Animator.SetTrigger("ThrowKunai");
            yield return new WaitForSeconds(delay);
            CmdThrowSmokeBomb(smokeBombSpawnPOS.transform.position, smokeBombSpawnPOS.transform.rotation);
        }
        yield return null;
    }

    [Command]
    void CmdThrowSmokeBomb(Vector3 position, Quaternion rotation)
    {
            GameObject smokeBomb = Instantiate(smokeBombPrefab, position, rotation);
            smokeBomb.transform.rotation = Quaternion.LookRotation(smokeBomb.transform.right, smokeBomb.transform.up);
            smokeBomb.GetComponent<Rigidbody>().velocity = -smokeBomb.transform.right * throwForce;
            m_CurrentSmokeBombs--;
            Destroy(smokeBomb, 10);
            NetworkServer.Spawn(smokeBomb.gameObject);

    }

    //changes the health UI for each individual player instead of just the host
    void OnChangeAmmo(int currentAmmo)
    {
        if (!isLocalPlayer)
            return;
        currentSmokeBombUICounter = GameObject.FindGameObjectWithTag("SmokeBombCounter").GetComponent<Text>();
        currentSmokeBombUICounter.text = currentAmmo.ToString();

    }



    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        if (!isLocalPlayer || isPaused)
            return;
        //getting the horizontal and vertical movement values
        float h = CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime;
        float v = CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime;

        ////handles the mouse movement
        m_CurrentX += Input.GetAxis("Mouse X") * mouseSensitivityX;
        m_CurrentX += Input.GetAxis("Right Stick X") * mouseSensitivityX;

        m_CurrentY -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        m_CurrentY -= Input.GetAxis("Right Stick Y") * mouseSensitivityY;
        m_CurrentY = Mathf.Clamp(m_CurrentY, cameraMinAngleY, cameraMaxAngleY);

        //determining the direction the player is trying to move in
        Vector3 directionVector = v * mainCamera.forward + h * mainCamera.right;
        if (directionVector.magnitude > 1f) directionVector.Normalize();
        directionVector = transform.InverseTransformDirection(directionVector);
        directionVector = Vector3.ProjectOnPlane(directionVector, Vector3.up);

        ////how much the player is trying to turn
        m_TurnAmount = Mathf.Atan2(directionVector.x, directionVector.z);

        ////determining if the player is standing still, walking, or running
        m_ForwardAmount = 0;

        if (v > 0)
        {
            m_ForwardAmount = 0.5f;
        }
        else if (v < 0)
        {
            m_ForwardAmount = -0.5f;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("LeftStickPress"))
        {
            m_ForwardAmount *= 2;
        }

        m_ForwardAmount = Mathf.Clamp(m_ForwardAmount, -1, 1);
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);

        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, m_ForwardAmount);
        m_TurnAmount = 0;
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime + m_CurrentX, 0);
        m_CurrentX = 0;

        mainCamera.parent.transform.rotation = transform.rotation;
        mainCamera.parent.transform.Rotate(m_CurrentY, 0, 0);

        Vector3 desiredCameraPos = mainCamera.TransformPoint(direction * maxDistance);
        RaycastHit hit;
        if (Physics.Linecast(mainCamera.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            mainCamera.localPosition = Vector3.Slerp(mainCamera.localPosition, direction * distance, Time.deltaTime * 10);
        }
        else
        {
            distance = maxDistance;
        }

        //setting the velocity of the character based on where the camera is facing
        Vector3 temp = m_ForwardAmount * transform.forward * moveSpeed;
        m_RigidBody.velocity = new Vector3(temp.x, m_RigidBody.velocity.y, temp.z);
    }
    public void TogglePauseMenu()
    {
        if (!isPaused)
        {
            isPaused = true;
            m_UIElements.SetActive(false);
        }
        else
        {
            isPaused = false;
            m_UIElements.SetActive(true);
        }
    }
}
