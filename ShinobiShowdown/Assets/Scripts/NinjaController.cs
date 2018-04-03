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

    [SerializeField] private Animator m_Animator;//holds the characters animation controller  
    [SerializeField] private Rigidbody m_RigidBody;//holds the characters rigidbody

    [SerializeField] private Transform mainCamera;//holds the camera object inside the player object
    [SerializeField] private Camera miniMapCamera;
    [SerializeField] private Transform throwableSpawnPOS;

    [SerializeField] private AudioSource m_WalkingSFXSource;
    [SerializeField] private AudioSource m_ThrowSFXSource;
    [SerializeField] private AudioClip throwSFX;
    [SerializeField] private AudioClip stabSFX;

    [SerializeField] private MeshRenderer miniMapIcon;



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
    private GameObject m_PauseMenu;
    public bool isPaused = false;
    private bool startTimer = false;
    private float timer = 0;

    [SerializeField] private GameObject smokeBombPrefab;
    [SerializeField] private float smokeBombThrowForce;
    [SyncVar(hook = "OnChangeSmokeBombAmmo")] private int m_CurrentSmokeBombs;
    public int CurrentSmokeBombAmmo { get { return m_CurrentSmokeBombs; } set { m_CurrentSmokeBombs = value; } }
    private Text currentSmokeBombUICounter;
    public int maxSmokeBombs;

    [SerializeField] private GameObject knifePrefab;//The prefab that holds the knife object
    [SerializeField] private float kunaiThrowForce;
    [SyncVar(hook = "OnChangeKunaiAmmo")] private int m_CurrentKunaiAmmo;//holds the current number of ammo that a player has
    public int CurrentAmmo { get { return m_CurrentKunaiAmmo; } set { m_CurrentKunaiAmmo = value; } }//lets other classes access the current number of kunai
    private Text currentKunaiUICounter;
    public int maxKunais;//how many kunais the player can carry

    void Start()
    {
        if (!isLocalPlayer)
        {
            mainCamera.GetComponent<Camera>().enabled = false;
            mainCamera.GetComponent<AudioListener>().enabled = false;
            miniMapCamera.enabled = false;
            miniMapIcon.enabled = false;
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        //setting up some variables 
        m_CurrentSmokeBombs = maxSmokeBombs;
        m_CurrentKunaiAmmo = maxKunais;
        m_UIElements = GameObject.FindGameObjectWithTag("UIElements");
        m_PauseMenu = GameObject.FindGameObjectWithTag("Pause Menu");
        m_PauseMenu.SetActive(false);
        direction = mainCamera.localPosition.normalized;
        distance = mainCamera.localPosition.magnitude;
        originalSpawnPos = transform.position;
    }

    private void Update()
    {

        if (!isLocalPlayer || isPaused)
            return;

        if (Input.GetButtonUp("Right Bumper") && !startTimer)
        {
            StartCoroutine(ThrowSmokeBombAfterDelay(0.4f));
            startTimer = true;
        }

        if ((Input.GetButtonDown("Fire1") || Input.GetAxisRaw("Right Trigger") != 0) && !startTimer)
        {
            //calls a command on the server to deal with the kunais across clients
            StartCoroutine(ThrowKunaiAfterDelay(0.4f));
            startTimer = true;
        }

        //A delay of 0.5 seconds before a player can throw their next smoke bomb
        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                timer = 0;
                startTimer = false;
            }
        }
    }

    IEnumerator ThrowSmokeBombAfterDelay(float delay)
    {
        if (m_CurrentSmokeBombs > 0)
        {
            m_Animator.SetTrigger("ThrowSmokeBomb");
            m_ThrowSFXSource.clip = throwSFX;
            m_ThrowSFXSource.Play();
            yield return new WaitForSeconds(delay);
            CmdThrowSmokeBomb(throwableSpawnPOS.transform.position, throwableSpawnPOS.transform.rotation);
        }
        yield return null;
    }

    IEnumerator ThrowKunaiAfterDelay(float delay)
    {
        if (m_CurrentKunaiAmmo > 0)
        {
            m_Animator.SetTrigger("ThrowKunai");
            m_ThrowSFXSource.clip = throwSFX;
            m_ThrowSFXSource.Play();
            yield return new WaitForSeconds(delay);
            CmdThrowKunai(throwableSpawnPOS.transform.position, throwableSpawnPOS.transform.rotation);
        }
        yield return null;
    }

    void OnChangeKunaiAmmo(int currentAmmo)
    {
        if (!isLocalPlayer)
            return;

        currentKunaiUICounter = GameObject.FindGameObjectWithTag("KunaiCounter").GetComponent<Text>();
        currentKunaiUICounter.text = currentAmmo.ToString();
    }
    [Command]
    void CmdThrowKunai(Vector3 position, Quaternion rotation)
    {
        //creates a knife on the client and the server, as long as the player has enough ammo
        GameObject knife = Instantiate(knifePrefab, position, rotation);
        knife.transform.rotation = Quaternion.LookRotation(knife.transform.right, knife.transform.up);
        knife.GetComponent<Rigidbody>().velocity = -knife.transform.right * kunaiThrowForce;
        m_CurrentKunaiAmmo--;
        NetworkServer.Spawn(knife.gameObject);
    }

    void OnChangeSmokeBombAmmo(int currentAmmo)
    {
        if (!isLocalPlayer)
            return;

        currentSmokeBombUICounter = GameObject.FindGameObjectWithTag("SmokeBombCounter").GetComponent<Text>();
        currentSmokeBombUICounter.text = currentAmmo.ToString();
    }

    [Command]
    void CmdThrowSmokeBomb(Vector3 position, Quaternion rotation)
    {
        GameObject smokeBomb = Instantiate(smokeBombPrefab, position, rotation);
        smokeBomb.transform.rotation = Quaternion.LookRotation(smokeBomb.transform.up, smokeBomb.transform.forward);
        smokeBomb.GetComponent<Rigidbody>().velocity = smokeBomb.transform.up * smokeBombThrowForce;
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
        if (Input.GetButtonDown("Pause"))
        {
            TogglePauseMenu();
        }

        if (!isLocalPlayer || isPaused)
            return;

        //getting the horizontal and vertical movement values
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

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
        directionVector = Vector3.ProjectOnPlane(directionVector, transform.up);

        ////how much the player is trying to turn
        m_TurnAmount = h;

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
            m_WalkingSFXSource.pitch = 2;
            moveSpeed = 7.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetButtonUp("LeftStickPress"))
        {
            m_WalkingSFXSource.pitch = 1;
            moveSpeed = 6.5f;
        }

        if (m_ForwardAmount != 0 || m_TurnAmount != 0)
        {
            m_WalkingSFXSource.UnPause();
        }
        else
        {
            m_WalkingSFXSource.Pause();
        }

        m_Animator.SetFloat("Forward", Mathf.Abs(m_ForwardAmount), 0.1f, Time.deltaTime);
        m_TurnAmount = Mathf.Clamp(m_TurnAmount, -0.5f, 0.5f);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);

        transform.Rotate(0, m_CurrentX, 0);
        m_CurrentX = 0;

        mainCamera.parent.transform.rotation = transform.rotation;
        mainCamera.parent.transform.Rotate(m_CurrentY, 0, 0);

        Vector3 desiredCameraPos = mainCamera.TransformPoint(direction * maxDistance);
        RaycastHit hit;
        if (Input.GetButton("Fire2") || Input.GetAxisRaw("Left Trigger") != 0)
        {
            distance = 0.5f;
        }
        else
        {
            if (Physics.Linecast(mainCamera.position, desiredCameraPos, out hit))
            {
                distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
            else
            {
                distance = maxDistance;
            }
        }
        mainCamera.localPosition = Vector3.Slerp(mainCamera.localPosition, direction * distance, Time.deltaTime * 10);

        //setting the velocity of the character based on where the camera is facing
        Vector3 temp = ((m_ForwardAmount * transform.forward) + (m_TurnAmount * transform.right)) * moveSpeed;
        m_RigidBody.velocity = new Vector3(temp.x, m_RigidBody.velocity.y + temp.y, temp.z);
    }
    public void TogglePauseMenu()
    {
        if (!isPaused)
        {
            isPaused = true;
            m_UIElements.SetActive(false);
            m_PauseMenu.SetActive(true);
            m_PauseMenu.transform.GetChild(0).GetComponent<Button>().Select();
        }
        else
        {
            isPaused = false;
            m_UIElements.SetActive(true);
            m_PauseMenu.SetActive(false);
        }
    }
}
