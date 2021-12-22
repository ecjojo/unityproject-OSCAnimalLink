using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    int PlayerID =-1;
    public Text PlayerIDText;
    public Image PlayerImg;

    public Sprite P1Img;
    public Sprite P2Img;

    public float z_vec = 0;
    public float x_vec = 0;

    public List<bool> isKeyPressed;

    public bool isGameStart = true;

    public GameObject GameEnd;
    public GameObject YouWinObj;
    public GameObject YouLoseObj;
    public Text YouWinText;
    public Text YouLoseText;

    public AudioSource bullet_hit;
    public AudioSource take_obj;
    public AudioSource speed_up;
    public AudioSource speed_down;

    // Start is called before the first frame update

    private void Start()
    {
        PlayerID = PlayerOSCScript.instance.playerID;
        PlayerID += 1;
        PlayerOSCScript.instance.oscRef.SetAddressHandler("/YouWin", YouWin);
        PlayerOSCScript.instance.oscRef.SetAddressHandler("/YouLose", YouLose);
        PlayerOSCScript.instance.oscRef.SetAddressHandler("/OnBackToTitle", OnBackToTitle);
 
        PlayerIDText.text = "PLAYER " + PlayerID;

        if(PlayerID ==1)
        {
            PlayerImg.sprite = P1Img;
        }
        else if(PlayerID == 2)
        {
            PlayerImg.sprite = P2Img;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerOSCScript.instance.oscRef.inited)
        {
            if (PlayerOSCScript.instance.GameStart)
            {
                if (isKeyPressed[0])
                {
                    HasPressed();
                } 
            }

        OscMessage message = new OscMessage();
        message.address = "/PlayerMoveUpdate";
        message.values.Add(x_vec);
        message.values.Add(z_vec);
        PlayerOSCScript.instance.oscRef.Send(message);
        }
        
    }
    public void HasPressed()
    {
        OscMessage message = new OscMessage();
        message.address = "/HasPressEvent";
        PlayerOSCScript.instance.oscRef.Send(message);
    }

    public void OnMoveButtonDown(int key)
    {
        isKeyPressed[key] = true;
    }

    public void OnMoveButtonUp(int key)
    {
        isKeyPressed[key] = false;
        OscMessage message = new OscMessage();
        message.address = "/NoHasPressEvent";
        PlayerOSCScript.instance.oscRef.Send(message);
    }

     public void YouWin(OscMessage message)
     {
        GameEnd.SetActive(true);
        YouWinObj.SetActive(true);
        YouWinText.text = "The best cupcake hunter is player " + PlayerID;
        isGameStart = false;
     }
    
     public void YouLose(OscMessage message)
     {
        GameEnd.SetActive(true);
        YouLoseObj.SetActive(true);
        if (PlayerID == 1)
        {
            YouLoseText.text = "The best cupcake hunter is player " + 2;
        }
        else if (PlayerID == 2)
        {
            YouLoseText.text = "The best cupcake hunter is player " + 1;
        }
        isGameStart = false;
     }

    public void OnBackToTitle(OscMessage message)
    {
        PlayerOSCScript.instance.oscRef.Close();
        Destroy(PlayerOSCScript.instance.gameObject);
        SceneManager.LoadScene("LogoScene");
    }

}
