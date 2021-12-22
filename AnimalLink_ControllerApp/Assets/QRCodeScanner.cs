using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing.QrCode;
using ZXing;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

public class QRCodeScanner : MonoBehaviour
{
    public static QRCodeScanner instance;

    WebCamTexture camTexture;
    public RawImage scanImage;

    public Text resultText;

    WebCamDevice[] devices;
    public Dropdown cameraSelector;

    public InputField LocalIPInput;
    public OSC titleOSCref;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        devices = WebCamTexture.devices; 

        for(int i = 0; i < devices.Length; i++)
        {
            cameraSelector.options.Add(new Dropdown.OptionData(devices[i].name));
        }

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

                    return  address.Address.ToString();
                }
            }
        }
        return "";
    }

    public void OnCameraChange(int i)
    {
        camTexture = new WebCamTexture(devices[i].name);
        camTexture.requestedHeight = 300;
        camTexture.requestedWidth = 300;

        scanImage.material.mainTexture = camTexture;
        scanImage.SetMaterialDirty();

        StartCamera();
    }

    public void StartCamera()
    {
        if (camTexture != null)
        {
            camTexture.Play();
        }
        StartCoroutine(UpdateCam());
    }

    IEnumerator UpdateCam()
    {
        while (true)
        {
            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
                if (result != null)
                {
                    //resultText.text = result.Text;
                    string[] datas = result.Text.Split(',');

                    initPlayerOSC(datas[0], int.Parse(datas[1]));
                    initTitleOSC(datas[0], int.Parse(datas[1]));

                    camTexture.Stop();

                    break;
                }
            }
            catch(Exception e) { Debug.Log(e.Message); }

            yield return new WaitForSeconds(1);
        }
    }

    public void initPlayerOSC(string IPAddress,int PlayerID)
    {
        PlayerOSCScript.instance.oscRef.outIP = IPAddress;
        PlayerOSCScript.instance.oscRef.outPort = 60000 + PlayerID + 1;
        PlayerOSCScript.instance.oscRef.inPort = 61000 + PlayerID + 1;

        PlayerOSCScript.instance.playerID = PlayerID;

        PlayerOSCScript.instance.oscRef.init();

        PlayerOSCScript.instance.setMessageHandlers();

        resultText.text = "P" + (PlayerID + 1);
    }

    public void initTitleOSC(string IPAddress, int PlayerID)
    {
        titleOSCref.outIP = IPAddress;
        titleOSCref.inPort = 63000 + PlayerID + 1;
        titleOSCref.init();

        Invoke("SendStartMessage", 1);
    }

    public void SendStartMessage()
    {
        OscMessage message = new OscMessage();
        message.address = "/OnPlayerJoined";
        message.values.Add(LocalIPInput.text);
        message.values.Add(PlayerOSCScript.instance.playerID);

        titleOSCref.Send(message);
    }

}
