using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    public bool isPressedKey = false;

    public GameObject TitlePage;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.anyKey & !isPressedKey)
        {
            TitlePage.GetComponent<Animator>().Play("TITLECK");
            isPressedKey = true;
        }
    }
}
