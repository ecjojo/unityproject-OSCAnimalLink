using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickSfx : MonoBehaviour
{
    public AudioSource Click_Sfx;

    public void ClickSound()
    {
        Click_Sfx.Play();
    }
}
