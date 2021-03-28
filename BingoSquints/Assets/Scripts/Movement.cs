using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    private Collision coll;
    public SpriteRenderer spriteRenderer;
    public Animator anim;


    [Space]
    [Header("Stats")]
    public float speed;
    public float slideSpeed;
    public float jumpForce;
    public float wallJumpVerticalForce;
    public float wallJumpHorizontalForce;
    public float wallJumpDuration;
    public float wallJumpGrav;
    private float inpSensitivity = .2f;
 
    private float usualGravity;

    private float wallJumpTimeElapsed = 0;
    public float wallJumpLerpDuration = 2;

    private float wallSlideTimeElapsed = 0;
    public float wallSlideLerpDuration = 1;
    

    [Space]
    [Header("Booleans")]
    public bool isGrounded;
    public bool isWallSliding;
    public bool isWallJumping;
    public int side = 1; //TODO: Use for animations
    public bool hasHorizontalInput;
    
    [Space]
    [Header("Better Jumping")]
    public bool betterJumpingEnabled = true;
    public float fallMultiplier = 2.5f;
    // public float lowJumpMultiplier = 2f;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        usualGravity = rb.gravityScale;
        // spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        hasHorizontalInput = (x > inpSensitivity || x < -inpSensitivity);
        
        Vector2 dir = new Vector2(x,y);
        Run(dir);
        Flip();

        
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
        
        // Debug.Log("main loop");
        //TODO: Removed for now, causing problems
        // Better Jumping Physics: 
        if (betterJumpingEnabled){ 
            //If the player is falling, fall faster according to fallMultiplier
            if(rb.velocity.y < 0) //If falling
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
            }
            //If the player is jumping, they dont jump as far unless they hold the jump button (acording to lowJumpMultiplier)
            // else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
            // {
            //     rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
            // }
        }
    }




    /** ======= Transition functions ========= **/

    //Move the character
    private void Run(Vector2 dir){

        if(coll.onGround && hasHorizontalInput){
            anim.SetBool("walking", true);
        }else{
            anim.SetBool("walking", false);
        }


        Vector2 runVelocity = new Vector2(dir.x * speed, rb.velocity.y);
        if(!isWallJumping)
        {
            //Regular move
            rb.velocity = runVelocity;
        }
        else if (isWallJumping){
            //Just after wall jumping, limit movement
            wallJumpTimeElapsed += Time.deltaTime;
            float lerpvalue = wallJumpTimeElapsed/wallJumpLerpDuration;
            rb.velocity = Vector2.Lerp(rb.velocity, runVelocity, lerpvalue);
        }
    }


    //Jump
    private void Jump(Vector2 dir)
    {
        //Call Jump animation
        
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce; 
    }

    //Walljump
    private void WallJump(Vector2 dir) //TODO: In progress
    {
        betterJumpingEnabled = false;
        isWallJumping = true;

        StartCoroutine(WallJumpingTimer(wallJumpDuration));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        rb.velocity = Vector2.up*wallJumpVerticalForce + wallDir*wallJumpHorizontalForce;
    }


    //The player slides down the wall this frame
    private void WallSlide()
    {
        isWallJumping = false;
        
        if(!isWallSliding){ //Start of the slide
            isWallSliding = true;
            wallSlideTimeElapsed = 0;
        }

        wallSlideTimeElapsed += Time.deltaTime;
        rb.gravityScale = 0;

        Vector2 slideVelocity = new Vector2(rb.velocity.x, -slideSpeed);
        Vector2 playerVelocity = rb.velocity;
        rb.velocity = Vector2.Lerp(playerVelocity, slideVelocity, wallSlideTimeElapsed / wallSlideLerpDuration);
    }
    private void EndWallSlide()
    {
        isWallSliding = false;
        rb.gravityScale = usualGravity;
    }
    

    //Player touches or leaves the ground - triggers once
    private void TouchGround(){
        isGrounded = true;
        betterJumpingEnabled = true;
        isWallJumping = false;
    }
    private void LeaveGround(){
        isGrounded = false;
    }


    //** ========= Helper Functions ========= **//

    IEnumerator WallJumpingTimer(float time)
    {
        wallJumpTimeElapsed = 0;
        isWallJumping = true;
        rb.gravityScale = wallJumpGrav;

        yield return new WaitForSeconds(time);

        rb.gravityScale = usualGravity;
        isWallJumping = false;
        betterJumpingEnabled = true;
    }

    //Via simmins
    void Flip()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal > 0)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            // facingRight = true;
            spriteRenderer.flipX = false;
        }
        else if (horizontal < 0)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            // facingRight = false;
            spriteRenderer.flipX = true;
        }
    }
}
