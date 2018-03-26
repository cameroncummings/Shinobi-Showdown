using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour {
    [SerializeField]private float durationInSeconds;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        durationInSeconds -= Time.deltaTime;
        if(durationInSeconds < 0)
        {
            EndRound();
            return;
        }
        gameObject.GetComponent<Text>().text = ((int)durationInSeconds / 60).ToString() + ":" + ((int)durationInSeconds % 60).ToString();


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
