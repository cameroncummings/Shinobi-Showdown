using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    private float timer;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage imageOfVideo;
    [SerializeField] private AudioSource MenuMusic;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0 && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            timer += Time.deltaTime;
            if(timer > 10.0f)
            {
                imageOfVideo.enabled = true;
                videoPlayer.enabled = true;
                videoPlayer.Play();

                MenuMusic.Pause();
            }
            
        }
        else
        {
            timer = 0;
            videoPlayer.time = 0f;
            videoPlayer.enabled = false;
            imageOfVideo.enabled = false;
            videoPlayer.Stop();
            MenuMusic.UnPause();
        }
        //Exits the game if you hit escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame_Clicked()
    {
        //loads the white box when you hit start scene
        SceneManager.LoadScene("LobbyUI");
    }
    public void OptionsBtn_Clicked()
    {
        //loads the options menu when you hit the options button
        SceneManager.LoadScene("OptionsMenu");
    }
    public void ExitGame_Clicked()
    {
        //Quits the game when you hit the quit button
        Application.Quit();
    }
}
