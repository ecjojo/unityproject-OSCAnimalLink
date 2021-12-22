using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int PlayerID;
	public Camera camera;
	public Vector3 move;
    public float moveSpeed = 3f;
    public bool HasJump = false;
    public AudioSource jumpsfx;

    public int JumpTime = 2;
    public float JumpPower = 1000f;
    int OriJumpTime = 0;
    int CountJumpTime = 0;
    bool[] HadJump;
    bool HaveJump = false;

    public int GameScore = 0;
    public Text GameScoreText;

    public Animator MyAni;
    public int myAnimod;
    
	OSC oscRef;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        oscRef = OSCManager.instance.PlayerOSCObjects[PlayerID];
        oscRef.SetAddressHandler("/PlayerMoveUpdate", OnPlayerMoveUpdate);
        oscRef.SetAddressHandler("/HasPressEvent", HasPressEvent);
        oscRef.SetAddressHandler("/NoHasPressEvent", NoHasPressEvent);
        rb = GetComponent<Rigidbody>();
        OriJumpTime = JumpTime;
        HadJump = new bool[JumpTime];
    }

    public void OnPlayerMoveUpdate(OscMessage message)
    {
        move = new Vector3(0f, 0f, (float)message.values[1]);
        float y = camera.transform.rotation.eulerAngles.y;
        move = Quaternion.Euler(0, y, 0) * move;
        gameObject.transform.Rotate(0, (float)message.values[0], 0);
    }

     public void HasPressEvent(OscMessage message)
     {
        HasJump = true;
     }

    public void NoHasPressEvent(OscMessage message)
    {
        HasJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);

        if (move.x != 0 || move.z != 0)
        {
            myAnimod = 1;
        }
        else
        {
            myAnimod = 0;
        }

        transform.Translate(move * Time.deltaTime * moveSpeed, Space.World);
        myAnimationController();

        if (HasJump && !HaveJump && !HadJump[JumpTime-1])
        {
            AddJump();
            HaveJump = true;
            rb.AddForce(new Vector3(0, JumpPower, 0));
            jumpsfx.Play();
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            ResetCountJump();
        } 
    }

    private void ResetCountJump()
    {
        JumpTime = OriJumpTime;
        CountJumpTime = 0;
        for (int i = 0; i < HadJump.Length; i++)
        {
            HadJump[i] = false;
        }
    }

    public void AddJump()
    {
        HadJump[CountJumpTime] = true;
        CountJumpTime += 1;
        Invoke("JumpMoreTime", 0.5f);
    }

    private void JumpMoreTime()
    {
        HaveJump = false;
    }

    public void Score()
    {
        GameScoreText.text = "Score: " + GameScore;
    }

    public void myAnimationController()
    {
        switch (myAnimod)
        {
            case 0:
                MyAni.Play("Idel");
                break;
            case 1:
                MyAni.Play("Walk");
                break;
            default:
                break;
        }
    }
}
