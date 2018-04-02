using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
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
