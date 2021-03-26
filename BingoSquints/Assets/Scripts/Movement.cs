using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    private Collision coll;


    [Space]
    [Header("Stats")]
    public float speed;
    public float jumpForce;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float slideSpeed = 5;
    // public float wallJumpLerp = 10;
    // public float dashSpeed = 20;

    [Space]
    [Header("Booleans")]
    public bool isWallSliding;
    public bool isDashing;
    private bool isGrounded;

    public bool wallJumped; //TODO:
    public int side = 1; //TODO:
    // public bool canMove; //Artifact

    // public bool wallGrab; 
    //TODO: Refactor this to match our code, we dont want wall grab


    void Start(){
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
    }

    void Update(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x,y);
        Run(dir);

        //Touching a wall and not the ground ==>> Slide down
        if(coll.onWall && !coll.onGround) 
        {
            WallSlide();
        }//When no longer wall sliding:
        else if (isWallSliding && (!coll.onWall || coll.onGround)){
            //Stop wall sliding
            // EndWallSlide();
            isWallSliding = false;
        }


        //When the player first touches the ground
        if (coll.onGround && !isGrounded)
        {
            TouchGround();
        }
        //When the player first leaves the ground
        else if(!coll.onGround && isGrounded){
            LeaveGround();
        }

        //Jump based on horizontal/vertical inputs
        if(Input.GetButtonDown("Jump"))
        {
            if(coll.onGround){
                Jump(dir);
            }
            else if (coll.onWall && !coll.onGround){
                WallJump(dir);
            }
        }


        //Better Jumping Physics: 
        //If the player is falling, fall faster according to fallMultiplier
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //If the player is jumping, they dont jump as far unless they hold the jump button (acording to lowJumpMultiplier)
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }






    /** The following functions are changes in the player's state**/

    private void Run(Vector2 dir){
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce; 
    }

    private void WallJump(Vector2 dir)
    {
        //TODO: Implement wall jumping
    }


    //The player slides down the wall this frame
    private void WallSlide()
    {
        isWallSliding = true;
        rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
    }


    //Player touches or leaves the ground - triggers once
    private void TouchGround(){
        isGrounded = true;
    }
    private void LeaveGround(){
        isGrounded = false;
    }
}
