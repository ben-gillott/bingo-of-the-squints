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
    public Vector3 bottomPoint, bottomSize;
    public Vector3 leftPoint, leftSize;
    public Vector3 rightPoint, rightSize;

    private Color debugCollisionColor = Color.red;

    //Update the value of collision states based on a series of circle colliders
    void Update()
    {  
        Collider[] onGroundColls = Physics.OverlapBox((Vector3)transform.position + bottomPoint, bottomSize/2, Quaternion.identity, groundLayer);
        onGround = (onGroundColls.Length > 0);
        
        Collider[] onLeftWallColl = Physics.OverlapBox((Vector3)transform.position + leftPoint, leftSize/2, Quaternion.identity, groundLayer);        
        onLeftWall = (onLeftWallColl.Length > 0);

        Collider[] onRightWallColl = Physics.OverlapBox((Vector3)transform.position + rightPoint, rightSize/2, Quaternion.identity, groundLayer);
        onRightWall = (onRightWallColl.Length > 0);
        
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
