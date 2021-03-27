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
    public float slideSpeed;
    public float jumpForce;
    public float wallJumpVerticalForce;
    public float wallJumpHorizontalForce;
    public float wallJumpDuration;
    public float wallJumpGrav;
    public float inpSensitivity;

    //Smaller value : longer time with less control after wall jump
    //Larger value : less time without control after wall jump
    public float wallJumpLerp = 10; 

    private float usualGravity;
    
    

    [Space]
    [Header("Booleans")]
    public bool isGrounded;
    public bool isWallSliding;
    public bool isWallJumping;
    public int side = 1; //TODO: Use for animations
    private bool canMove = true;
    public bool hasHorizontalInput;
    // [Space]
    // [Header("Better Jumping")]
    // private bool betterJumpingEnabled = true;
    // public float fallMultiplier = 2.5f;
    // public float lowJumpMultiplier = 2f;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        usualGravity = rb.gravityScale;
    }


    void Update(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        hasHorizontalInput = (x > inpSensitivity || x < -inpSensitivity);
        
        Vector2 dir = new Vector2(x,y);
        Run(dir);
        

        
        //Wall sliding this frame
        //TODO: only stick if appropriate side, check col right and left
        if(coll.onWall && !coll.onGround && hasHorizontalInput)  
        {
            WallSlide();
        }//No longer wall sliding for the first time
        else if (isWallSliding  && (!coll.onWall || coll.onGround || !hasHorizontalInput)){
            EndWallSlide();
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
        
        
        //TODO: Removed for now, causing problems
        //Better Jumping Physics: 
        // if (betterJumpingEnabled){ 
        //     //If the player is falling, fall faster according to fallMultiplier
        //     if(rb.velocity.y < 0)
        //     {
        //         rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        //     }
        //     //If the player is jumping, they dont jump as far unless they hold the jump button (acording to lowJumpMultiplier)
        //     else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        //     {
        //         rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        //     }
        // }
    }



    /** ======= Transition functions ========= **/


    //Move the character
    private void Run(Vector2 dir){
        if(!canMove){
            return;
        }

        if(!isWallJumping)
        {
            //Regular move
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        }
        else if (isWallJumping){
            //Normally:
            // rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
            //Just after wall jumping, limit movement
            // rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }


    //Jump
    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce; 
    }

    //Walljump
    private void WallJump(Vector2 dir) //TODO: In progress
    {
        // betterJumpingEnabled = false;
        isWallJumping = true;

        StartCoroutine(DisableMovement(wallJumpDuration));
        StartCoroutine(WallJumpingTimer(wallJumpDuration));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        rb.velocity = Vector2.up*wallJumpVerticalForce + wallDir*wallJumpHorizontalForce;
    }


    //The player slides down the wall this frame, or stops wall sliding
    private void WallSlide()
    {
        if (!canMove)
            return;

        isWallSliding = true;
        rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
    }
    private void EndWallSlide()
    {
        isWallSliding = false;
    }
    

    //Player touches or leaves the ground - triggers once
    private void TouchGround(){
        isGrounded = true;
        // betterJumpingEnabled = true;
        isWallJumping = false;
    }
    private void LeaveGround(){
        isGrounded = false;
    }


    //** ========= Helper Functions ========= **//

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    IEnumerator WallJumpingTimer(float time)
    {
        isWallJumping = true;
        rb.gravityScale = wallJumpGrav;

        yield return new WaitForSeconds(time);

        rb.gravityScale = usualGravity;
        isWallJumping = false;
    }
}
