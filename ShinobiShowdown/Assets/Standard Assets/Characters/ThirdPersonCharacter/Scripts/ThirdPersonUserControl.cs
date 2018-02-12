using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : NetworkBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        public Transform m_MainCam;                  // A reference to the main camera in the scenes transform
        public Camera miniMapCam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_MainCamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        private bool pausedGame = false;

        private void Start()
        {
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();

            // DISABLE CAMERA AND CONTROLS HERE (BECAUSE THEY ARE NOT ME)
            if (isLocalPlayer)
                return;

            m_MainCam.GetComponentInChildren<Camera>().enabled = false;
            m_MainCam.GetComponentInChildren<AudioListener>().enabled = false;
            miniMapCam.GetComponentInChildren<MeshRenderer>().enabled = false;
            miniMapCam.enabled = false;
        }

        private void Update()
        {
            if (pausedGame)
                return;

            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {

            if (!isLocalPlayer || pausedGame)
                return;
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_MainCam != null)
            {
                // calculate camera relative direction to move:
                m_MainCamForward = Vector3.Scale(m_MainCam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_MainCamForward + h * m_MainCam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;


        }

        public void PauseGame(bool isPaused)
        {
                pausedGame = isPaused;
        }
    }
}
