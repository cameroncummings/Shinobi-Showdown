using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class WinLoseScreenBehaivior : MonoBehaviour
{
    private GameObject networkGameObject;

    private void Start()
    {
        networkGameObject = GameObject.FindGameObjectWithTag("NetworkManager");
    }

    public void StartNewGame()
    {
        Destroy(networkGameObject);
        NetworkManager.Shutdown();
        SceneManager.LoadScene("LobbyUI");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
