using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOSCScript : MonoBehaviour
{

    public static PlayerOSCScript instance;
    public OSC oscRef;
    public int playerID;
    public bool GameStart=false;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        oscRef = GetComponent<OSC>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setMessageHandlers()
    {
        oscRef.SetAddressHandler("/GameStart",OnGameStart);
    }

    public void OnGameStart(OscMessage message)
    {
        SceneManager.LoadScene("GameScene");
        GameStart = true;
    }

    public void ChangePlayerID(int InputPlayerID)
    {
        playerID = InputPlayerID;
    }
}
