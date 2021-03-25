using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;

    [Space]
    [Header("Stats")]
    public float speed;
    public float jumpForce;
    // public float slideSpeed = 5;
    // public float wallJumpLerp = 10;
    // public float dashSpeed = 20;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x,y);

        Walk(dir);

        if(Input.GetKeyDown("space")){
            Jump(dir);
        }

    }

    private void Walk(Vector2 dir){
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce; 
    }
}
