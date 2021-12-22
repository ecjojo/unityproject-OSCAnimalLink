using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public GameObject circle;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(circle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
