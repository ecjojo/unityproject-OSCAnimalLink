using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIManager : MonoBehaviour
{
    public bool isPressedKey = false;

    public GameObject logo;
    public GameObject TitleTip;
    public GameObject MeunButton;
    public GameObject TitlePanel;
    public GameObject HowPanel;
    public GameObject StartPanel;

    void Start()
    {
    }

    void Update()
    {
        if(Input.anyKey & !isPressedKey)
        {
            TitleTip.SetActive(false);
            logo.GetComponent<Animator>().Play("logoformeun");
            MeunButton.SetActive(true);
            isPressedKey = true;
        }
    }

    public void Button_OpenStart()
    {
        TitlePanel.SetActive(false);
        MeunButton.SetActive(false);
    }

    public void Button_OpenHow()
    {
        HowPanel.SetActive(true);
    }

    public void Button_BackMeun()
    {
        TitlePanel.SetActive(true);
        MeunButton.SetActive(true);
        HowPanel.SetActive(false);
    }

    public void Button_ExitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
