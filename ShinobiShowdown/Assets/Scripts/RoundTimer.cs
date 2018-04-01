using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour {
    [SerializeField]private float timer;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float minutes = Mathf.Floor(timer / 60);
        float seconds = Mathf.RoundToInt(timer % 60);

        string timerString = "";

        if(minutes < 10)
            timerString = "0" + minutes.ToString();
        else
            timerString = minutes.ToString();
        if (seconds < 10)
            timerString += ":0" + seconds.ToString();
        else
            timerString += ":" + seconds.ToString();
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            EndRound();
            return;
        }
        gameObject.GetComponent<Text>().text = timerString;


	}

    void EndRound()
    {
        //Reset Timer
        //Reset Player Positions
        //if three rounds have ended end the game
        //Destroy Players
        //Display Win Loss Message

    }
}
