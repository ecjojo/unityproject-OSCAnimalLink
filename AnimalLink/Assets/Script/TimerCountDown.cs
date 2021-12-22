using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountDown : MonoBehaviour
{
    public Text timerText;
    public float time = 180;    //(60 x ? minutes) e.g. 3 minutes =180

    void Start()
    {
        StartCoundownTimer();
    }

    void StartCoundownTimer()
    {
        if (timerText != null)
        {
            time = 180;
            timerText.text = "Time Left: 03:00:000";
            InvokeRepeating("UpdateTimer", 0.0f, 0.01667f);
        }
    }

    void UpdateTimer()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
            string minutes = Mathf.Floor(time / 60).ToString("00");
            string seconds = (time % 60).ToString("00");
           
            timerText.text = "Time : " + minutes + ":" + seconds;
        }
        else if (time <= 0)
        {
            time = 0;
        }
    }

    /*public GameObject textDisplay;
    public int secondLeft = 30;
    public bool takingAway = false;

    void Start()
    {
        textDisplay.GetComponent<Text>().text = "00:" + secondLeft;
    }

    void Update()
    {
        if (takingAway == false && secondLeft > 0)
        {
            StartCoroutine(TimerTake());
        }
    }

    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondLeft -= 1;
        if (secondLeft < 10)
        {
            textDisplay.GetComponent<Text>().text = "00:0" + secondLeft;
        }
        else
        {
textDisplay.GetComponent<Text>().text = "00:" + secondLeft;
        }
        
        takingAway = false;
;
    }*/
}
