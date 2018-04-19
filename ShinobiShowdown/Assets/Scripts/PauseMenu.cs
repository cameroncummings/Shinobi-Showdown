using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public void Resume()
    {
        FindObjectOfType<NinjaController>().TogglePauseMenu();
        //gameObject.SetActive(false);
    }
    public void MainMenu()
    {
        GameObject networkGameObject = GameObject.FindGameObjectWithTag("NetworkManager");
        Destroy(networkGameObject);
        NetworkManager.Shutdown();
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
