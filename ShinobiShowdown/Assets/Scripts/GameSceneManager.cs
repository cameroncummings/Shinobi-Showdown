using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private Text indicatorText;
    /*[SyncVar(hook = "OnTimerToggle")]*/ private bool m_ShowTimer = false;
    private float currentProgress = 10;


    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //progressBar.fillAmount = currentProgress / 10;
        //int displayNumber = (int)currentProgress;
        //indicatorText.text = displayNumber.ToString();

        //currentProgress -= Time.deltaTime;

    }

    //void OnTimerToggle(bool showTimer)
    //{
    //    progressBar.transform.parent.GetComponent<GameObject>().SetActive(showTimer);
    //}
}
