using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public InputField IPInputField;
    public string LocalIPInput;

    public void ConnectButtonOnClick(int PlayerID)
    {
        if (!PlayerOSCScript.instance.oscRef.inited)
        {
            PlayerOSCScript.instance.oscRef.outIP = IPInputField.text;
            PlayerOSCScript.instance.oscRef.outPort = 60000 + PlayerID + 1;
            PlayerOSCScript.instance.oscRef.inPort = 61000 + PlayerID + 1;

            PlayerOSCScript.instance.oscRef.init();

            PlayerOSCScript.instance.setMessageHandlers();

            PlayerOSCScript.instance.ChangePlayerID(PlayerID);

            QRCodeScanner.instance.initTitleOSC(IPInputField.text, PlayerID);
            Debug.Log("Yes");
        }
    }
}
