using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector]
    // public Rigidbody2D rb;
    public Rigidbody rb;

    private Collision coll;
    // private SoundManager soundManager;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public AudioSource walkAudio;

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
 
    public Vector3 usualGravity;

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
        Physics.gravity = usualGravity;
        rb = GetComponent<Rigidbody>();

        coll = GetComponent<Collision>();
        // spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        hasHorizontalInput = (x > inpSensitivity || x < -inpSensitivity);
    


        if(coll.onGround){
            anim.SetBool("grounded", true);
            anim.ResetTrigger("jump");
            anim.ResetTrigger("walljump");
        }else{
            anim.SetBool("grounded", false);
        }

        if(hasHorizontalInput){
            anim.SetBool("hasinput", true);
        }else{
            anim.SetBool("hasinput", false);
        }

        if(isWallSliding){
            anim.SetBool("onwall", true);
            anim.ResetTrigger("walljump");
        }else{
            anim.SetBool("onwall", false);
        }

        if(isWallJumping){
            anim.SetTrigger("walljump");
        }else{
            anim.ResetTrigger("walljump");
        }

        if(coll.onGround && (rb.velocity.x != 0)){// !walkAudio.isPlaying){
            walkAudio.Play();
            Debug.Log("Should play" + rb.velocity.x );
        }else{
            walkAudio.Pause();
        }

        Vector2 dir = new Vector2(x,y);
        Run(dir);
        Flip();
        
        //Wall sliding this frame
        //TODO: only stick if appropriate side, check col right and left
        if(coll.onWall && !coll.onGround)  
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
            SoundManagerScript.PlaySound("jumpSound");

            if(coll.onGround){
                anim.SetTrigger("jump");

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
                // rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier) * Time.deltaTime;
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
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        rb.velocity += Vector3.up * jumpForce; 
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
        if(!isWallSliding){ //Start of the slide
            SoundManagerScript.PlaySound("landingSound");
            isWallSliding = true;
            wallSlideTimeElapsed = 0;
        }
        

        wallSlideTimeElapsed += Time.deltaTime;

        Vector3 slideVelocity = new Vector3(rb.velocity.x, -slideSpeed, 0);
        Vector3 playerVelocity = rb.velocity;

    //TODO: Why does it stick to the wall
        //Lerps between player velocity and a negative wall slide speed to give the juicy katana zero feel
        rb.velocity = Vector3.Lerp(playerVelocity, slideVelocity, wallSlideTimeElapsed / wallSlideLerpDuration);
    }

    private void EndWallSlide()
    {
        isWallSliding = false;
        Physics.gravity = usualGravity;
    }
    

    //Player touches or leaves the ground - triggers once
    private void TouchGround(){
        SoundManagerScript.PlaySound("landingSound");
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
        Physics.gravity = new Vector3(0f, wallJumpGrav, 0f);

        yield return new WaitForSeconds(time);

        Physics.gravity = usualGravity;
        isWallJumping = false;
        betterJumpingEnabled = true;
    }

    //Via simmins
    void Flip()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal < 0)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            // facingRight = true;
            spriteRenderer.flipX = false;
        }
        else if (horizontal > 0)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            // facingRight = false;
            spriteRenderer.flipX = true;
        }
    }
}
