using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour {
    [SerializeField]private float duration;

	// Update is called once per frame
	void Update ()
    {
        float minutes = Mathf.Floor(duration / 60);
        float seconds = Mathf.RoundToInt(duration % 60);

        string timerString = "";

        if(minutes < 10)
            timerString = "0" + minutes.ToString();
        else
            timerString = minutes.ToString();
        if (seconds < 10)
            timerString += ":0" + seconds.ToString();
        else
            timerString += ":" + seconds.ToString();
        duration -= Time.deltaTime;

        if(duration < 0)
        {
            EndRound();
            return;
        }
        gameObject.GetComponent<Text>().text = timerString;


	}

    void EndRound()
    {
        SceneManager.LoadScene("DefenseWin");
    }
}
