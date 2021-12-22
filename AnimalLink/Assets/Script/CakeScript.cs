using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeScript : MonoBehaviour
{
    public int Score;

    GameController GameController;

    void Start()
    {
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
        GameController.CakeNumber += 1;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController PlayerController = collision.gameObject.GetComponent<PlayerController>();
            PlayerController.GameScore += Score;
            PlayerController.Score();
            Destroy(gameObject);
            GameController.CakeNumber -= 1;
        }
    }
}
