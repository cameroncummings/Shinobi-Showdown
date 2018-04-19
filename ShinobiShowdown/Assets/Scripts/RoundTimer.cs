using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RoundTimer : NetworkBehaviour {
    [SyncVar (hook = "OnTimerTick")]private float duration;

    private void Start()
    {
        duration = 120;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-73.5f, -59, 0);
        gameObject.GetComponent<Text>().color = Color.white;
    }

    // Update is called once per frame
    void Update ()
    {
        duration -= Time.deltaTime;

        if(duration < 0)
        {
            EndRound();
            return;
        }
	}

    void OnTimerTick(float timeRemaining)
    {
        float minutes = Mathf.Floor(timeRemaining / 60);
        float seconds = Mathf.RoundToInt(timeRemaining % 60);

        string timerString = "";

        if (minutes < 10)
            timerString = "0" + minutes.ToString();
        else
            timerString = minutes.ToString();
        if (seconds < 10)
            timerString += ":0" + seconds.ToString();
        else
            timerString += ":" + seconds.ToString();

        gameObject.GetComponent<Text>().text = timerString;
    }

    void EndRound()
    {
        SceneManager.LoadScene("DefenseWin");
    }
}
