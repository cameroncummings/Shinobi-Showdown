using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private float timer;
    public Image videoScreen;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0 && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            timer += Time.deltaTime;
            if(timer > 10.0f)
            {
                videoScreen.enabled = true;
            }
            
        }
        else
        {
            timer = 0;
            videoScreen.enabled = false;
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
