using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCakeScript : MonoBehaviour
{
    public GameObject[] Cake;

    public List<GameObject> CakePos;
    
    public GameObject CakePrefab;

    public bool HaveCallCake = false;

    // Start is called before the first frame update
    void Start()
    {
        Cake = GameObject.FindGameObjectsWithTag("Cake");
        
        for(int i = 0; i < Cake.Length; i++)
        {
            CakePos[i].transform.position = Cake[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!HaveCallCake && GameObject.Find("GameController").GetComponent<GameController>().CakeNumber == 0)
        {
            Invoke("CakeSpawnPoint", 1.5f);
            HaveCallCake = true;
        }
        
        if(HaveCallCake && GameObject.Find("GameController").GetComponent<GameController>().CakeNumber > 0)
        {
            HaveCallCake = false;
        }
        
    }

    public void CakeSpawnPoint()
    {
        int[] RandmonNumber = new int[4];

        for (int i=0;i< 4; i++)
        {
            RandmonNumber[i] = Random.Range(0, 8);

            if (i > 0)
            {
                if (i == 1)
                {
                    while (RandmonNumber[i] == RandmonNumber[0] || RandmonNumber[i] == RandmonNumber[2] || RandmonNumber[i] == RandmonNumber[3])
                    {
                        RandmonNumber[i] = Random.Range(0, 8);
                    }
                }
                else if(i == 2)
                {
                    while (RandmonNumber[i] == RandmonNumber[0] || RandmonNumber[i] == RandmonNumber[1] || RandmonNumber[i] == RandmonNumber[3])
                    {
                        RandmonNumber[i] = Random.Range(0, 8);
                    }
                }
                else if (i == 3)
                {
                    while (RandmonNumber[i] == RandmonNumber[0] || RandmonNumber[i] == RandmonNumber[1] || RandmonNumber[i] == RandmonNumber[2])
                    {
                        RandmonNumber[i] = Random.Range(0, 8);
                    }
                }
            }
            Instantiate(CakePrefab, CakePos[RandmonNumber[i]].transform.position, Quaternion.identity);
        }
        HaveCallCake = false;
    }
}
