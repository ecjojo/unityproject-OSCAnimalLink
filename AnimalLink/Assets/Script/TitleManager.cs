using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

public class TitleManager : MonoBehaviour
{
    public List<InputField> IPInputFields;
    public List<Image> QRCodeImgs;
    public List<GameObject> ReadyIcons;

    public InputField LocalIPInput;
    public string LocalIP;

    public OSC titleOSCref;

    private void Start()
    {
        LocalIP =GetLocalIP();
        UpdateQRCode();

        titleOSCref.init();
        titleOSCref.SetAddressHandler("/OnPlayerJoined", OnPlayerJoined);

        LocalIPInput.text = GetLocalIP();
        LocalIPInput.placeholder.GetComponent<Text>().text = LocalIPInput.text;
    }

    public string GetLocalIP()
    {

        // Get a list of all network interfaces (usually one per network card, dialup, and VPN connection)
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface network in networkInterfaces)
        {
            // Read the IP configuration for each network
            IPInterfaceProperties properties = network.GetIPProperties();

            if (network.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                   network.OperationalStatus == OperationalStatus.Up &&
                   !network.Description.ToLower().Contains("virtual") &&
                   !network.Description.ToLower().Contains("pseudo"))
            {
                // Each network interface may have multiple IP addresses
                foreach (IPAddressInformation address in properties.UnicastAddresses)
                {
                    // We're only interested in IPv4 addresses for now
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    // Ignore loopback addresses (e.g., 127.0.0.1)
                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    return LocalIP = address.Address.ToString();
                }
            }
        }
        return "";
    }

    public void UpdateQRCode()
    {
        for(int i = 0; i < 2; i++)
        {
            string s = "";
            s = LocalIP + "," + i;
            QRCodeImgs[i].material.mainTexture = QRCodeManager.GenQRCode(s, 256, 256);
            QRCodeImgs[i].SetMaterialDirty();
        }
    }

    private void Update()
    {
    }

    public void StartButtonOnClick()
    {
        for (int i = 0; i < 2; i++)
        {
            if (!OSCManager.instance.PlayerOSCObjects[i].inited && IPInputFields[i].text != "")
            {
                OSCManager.instance.PlayerOSCObjects[i].outIP = IPInputFields[i].text;
                OSCManager.instance.PlayerOSCObjects[i].init();
            }
            if (OSCManager.instance.PlayerOSCObjects[i].inited)
            {
                OscMessage message = new OscMessage();
                message.address = "/GameStart";
                OSCManager.instance.PlayerOSCObjects[i].Send(message);
            }
        }
        /*for (int i = 0; i < 2; i++)
        {
            if (OSCManager.instance.PlayerOSCObjects[i].inited)
            {
                OscMessage message = new OscMessage();
                message.address = "/GameStart";
                OSCManager.instance.PlayerOSCObjects[i].Send(message);
            }
        }*/

        SceneManager.LoadScene("GameScene");
    }

    public void OnPlayerJoined(OscMessage message)
    {
        string PlayerIP = message.values[0].ToString();
        int PlayerID = int.Parse(message.values[1].ToString());

        OSCManager.instance.PlayerOSCObjects[PlayerID].outIP = PlayerIP;
        OSCManager.instance.PlayerOSCObjects[PlayerID].init();

        ReadyIcons[PlayerID].SetActive(true);
        QRCodeImgs[PlayerID].gameObject.SetActive(false);

    }

    public void BackToStartMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }

}
