using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Escape))
        {
            //exits to the main menu
            SceneManager.LoadScene("MainMenu");
        }
	}

    public void BackBtn_Clicked()
    {
        //exits to the main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void ApplyBtn_Clicked()
    {
        //exits to the main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void OkBtn_Clicked()
    {
        //exits to the main menu
        SceneManager.LoadScene("MainMenu");
    }

}
