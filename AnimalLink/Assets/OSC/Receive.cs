using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Receive : MonoBehaviour {
    [Header("OSC腳本")]
    public OSC oscReference;

    void Start () {
        oscReference.SetAllMessageHandler(OnReceive);

    }

	void Update () {

    }

    void OnReceive(OscMessage message)
    {
        string data=message.values[0].ToString();
		Debug.Log (data);
    }

}