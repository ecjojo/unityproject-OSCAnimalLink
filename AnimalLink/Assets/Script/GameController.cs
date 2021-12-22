using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject[] Player;
    public AudioSource bgm;

    public GameObject ResultScence;
    public GameObject[] PlayerWinObj;
    public Text[] PlayerWinText;

    public int CakeNumber = 0;

    public bool GameEnd = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        bgm.Play();
    }

 
    void Update()
    {
        if (GameObject.Find("Timer").GetComponent<TimerCountDown>().time == 0 && !GameEnd)
        {
            GameEnd = true;
            Time.timeScale = 0;

            ResultScence.SetActive(true);
            if(Player[0].GetComponent<PlayerController>().GameScore > Player[1].GetComponent<PlayerController>().GameScore)
            {
                PlayerWinObj[0].SetActive(true);
                PlayerWinText[0].text = "Eaten cupcake :" + Player[0].GetComponent<PlayerController>().GameScore;
                OscMessage message = new OscMessage();
                message.address = "/YouWin";
                OSCManager.instance.PlayerOSCObjects[0].Send(message);
                OscMessage message2 = new OscMessage();
                message2.address = "/YouLose";
                OSCManager.instance.PlayerOSCObjects[1].Send(message2);
            }
            else if (Player[0].GetComponent<PlayerController>().GameScore < Player[1].GetComponent<PlayerController>().GameScore)
            {
                PlayerWinObj[1].SetActive(true);
                PlayerWinText[1].text = "Eaten cupcake :" + Player[1].GetComponent<PlayerController>().GameScore;
                OscMessage message = new OscMessage();
                message.address = "/YouWin";
                OSCManager.instance.PlayerOSCObjects[1].Send(message);
                OscMessage message2 = new OscMessage();
                message2.address = "/YouLose";
                OSCManager.instance.PlayerOSCObjects[0].Send(message2);
            }
        }
    }

    public void GoToJoinScene()
    {
        for(int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            OscMessage message = new OscMessage();
            message.address = "/OnBackToTitle";
            OSCManager.instance.PlayerOSCObjects[i].Send(message);
        }
        SceneManager.LoadScene("LogoScene");
        OSCManager.instance.Destroy();
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

}
