using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class WinLoseScreenBehaivior : MonoBehaviour
{

    public void StartNewGame()
    {
        SceneManager.LoadScene("LobbyUI");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
