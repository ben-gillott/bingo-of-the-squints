using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    Rigidbody2D rb2d;

    SpriteRenderer spriteRenderer;

    public float moveSpeed;
    public float jumpForce;
    public float dashForce;

    public Transform groundCheck;
    public Transform groundCheck_L;
    public Transform groundCheck_R;

    public Transform wallCheck_L;
    public Transform wallCheck_R;

    public bool grounded;
    public bool jumping;
    public bool facingRight;
    bool dashing;

    public bool attached_Left;
    public bool attached_Right;
    float attachedToWall = 5;

    bool wallJumping;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleInput();
        Flip();

        if (attached_Left)
        {
            attachedToWall = 1;
        }
        else if (attached_Right)
        {
            attachedToWall = -1;
        }

        if (wallJumping)
        {
            rb2d.velocity = new Vector2(moveSpeed * attachedToWall * Time.fixedDeltaTime / Time.timeScale, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        if ((Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")) ||
            (Physics2D.Linecast(transform.position, groundCheck_L.position, 1 << LayerMask.NameToLayer("Ground")) ||
            (Physics2D.Linecast(transform.position, groundCheck_R.position, 1 << LayerMask.NameToLayer("Ground"))))))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (jumping)
        {
            Jump();
        }

        if ((Physics2D.Linecast(transform.position, wallCheck_L.position, 1 << LayerMask.NameToLayer("wall"))))
        {
            attached_Left = true;
            //Debug.Log("attached to wall");
        }
        else
        {
            attached_Left = false;
        }

        if ((Physics2D.Linecast(transform.position, wallCheck_R.position, 1 << LayerMask.NameToLayer("wall"))))
        {
            attached_Right = true;
            //Debug.Log("attached to wall");
        }
        else
        {
            attached_Right = false;
        }
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if(horizontal == 0)
        {
            anim.SetBool("walk", false);
        }
        else
        {
            anim.SetBool("walk", true);
        }


        if (!dashing)
        {
            rb2d.velocity = new Vector2(horizontal * moveSpeed * Time.fixedDeltaTime / Time.timeScale, rb2d.velocity.y);
        }

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            jumping = true;
        }
        else
        {
            jumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && (attached_Right || attached_Left) && !grounded)
        {
            wallJumping = true;
            Invoke("ResetWallJump", 0.1f);
        }
    }

    void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        //Debug.Log("You're jumping");
    }

    public void Dash()
    {
        dashing = true;

        if (facingRight)
        {
            rb2d.velocity = new Vector2(dashForce, 0);
        }
        else if (!facingRight)
        {
            rb2d.velocity = new Vector2(-dashForce, 0);
        }
        Invoke("ResetDashing", 0.08f);
    }

    void ResetDashing()
    {
        dashing = false;
    }

    void ResetWallJump()
    {
        wallJumping = false;
    }

    void Flip()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal > 0)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
            spriteRenderer.flipX = false;
        }
        else if (horizontal < 0)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
            spriteRenderer.flipX = true;
        }
    }
}
