using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReMovement : MonoBehaviour
{
    public Rigidbody rb;
    public WallJumpDetection wjd;

    public BoxCollider groundCollider;

    public bool onWall;
    public int wallSide;
    public bool once;
    public bool faceSide;
    public bool isGrounded;
    public float speed;
    public float gravity;
    public float jumpForce;
    public float LaunchForce;
    Vector3 jumpV3 = new Vector3(0.0f, 1f, 0.0f);
    public float lx;
    public float ly;


    public float x;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Jump && isGrounded)
        {
            rb.AddForce(jumpV3 * jumpForce, ForceMode.Impulse);
        }

        if (Jump && onWall && once)
        {
            rb.AddForce(lx * wallSide, ly, 0 , ForceMode.VelocityChange);
            once = false;
            onWall = false;
            
        }

        x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(x, 0, 0);
        WallCheck();
        UpdateFacing(dir);
        Move(dir);
    }

    public bool Jump
    {
      get { return Input.GetKeyDown(KeyCode.Space); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Debug.Log("Ground");
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Debug.Log("Air");
            isGrounded = false;
        }
    }

    public void WallCheck()
    {
        if (x == 0 && onWall || isGrounded)
        {
            onWall = false;
            rb.useGravity = true;
        }
        else if (!isGrounded && !onWall)
        {
            rb.useGravity = true;
        }

       
    }

    private void UpdateFacing(Vector3 Direction)
    {
        if (Direction.x > 0)
        {
            wjd.UpdateRight();
            
        }
        else if (Direction.x < 0)
        {
            wjd.UpdateLeft();
            
        }
        else
        {
            //Idle
        }
    }

    public void WallGrab(bool WallSide)
    {
        if (WallSide && x < 0 && !isGrounded)
        {
            Debug.Log("LeftWallhug");
            wallSide = 1;
            WallStick();
        }
        else if (!WallSide && x > 0 && !isGrounded)
        {
            Debug.Log("Right");
            wallSide = -1;
            WallStick();
        }
        else
        {

        }
    }

    public void WallStick()
    {
            onWall = true;
    }

    public void ExitWall()
    {
        rb.useGravity = false;
        once = true;
    }

    private void Move(Vector2 dir)
    {
        Vector3 runVelocity = new Vector3(dir.x, dir.y, 0);
        if (onWall)
        {
            return;
        }
        else
        {
            rb.transform.Translate(runVelocity * speed * Time.deltaTime);
        }
    }

}
