using UnityEngine;
using System.Collections;

public class SendOSC : MonoBehaviour {


	public OSC oscReference;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.S))
        {
            OscMessage message = new OscMessage();
            message.address = "/sendToProcessing";
            message.values.Add("sned content to Porcessing");
            oscReference.Send(message);
        }
    }

	void OnMouseDown() {

	}
}
