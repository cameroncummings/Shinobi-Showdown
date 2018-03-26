using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkHUD : MonoBehaviour {

	// Update is called once per frame
    public void hostServer()
    {
        NetworkManager.singleton.StartHost();
    }

    public void joinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("joinIP").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    public void Back()
    {
        Application.LoadLevel("MainMenu");
    }
}
