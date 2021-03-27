using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]
    [Header("Statuses")]
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;

    [Space]
    [Header("Collision")]
    public Vector2 bottomPoint, bottomSize;
    public Vector2 leftPoint, leftSize;
    public Vector2 rightPoint, rightSize;

    private Color debugCollisionColor = Color.red;

    //Update the value of collision states based on a series of circle colliders
    void Update()
    {  
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomPoint, bottomSize, 0f, groundLayer);
        
        onLeftWall = Physics2D.OverlapBox((Vector2)transform.position + leftPoint, leftSize, 0f, groundLayer);
        
        onRightWall = Physics2D.OverlapBox((Vector2)transform.position + rightPoint, rightSize, 0f, groundLayer);

        onWall = onLeftWall || onRightWall;
        
        wallSide = onRightWall ? -1 : 1;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 bot3D = bottomPoint;
        Vector3 left3D = leftPoint;
        Vector3 right3D = rightPoint;

        Gizmos.DrawWireCube((bot3D + transform.position), bottomSize);
        Gizmos.DrawWireCube((left3D + transform.position), leftSize);
        Gizmos.DrawWireCube((right3D + transform.position), rightSize);
    }
}
