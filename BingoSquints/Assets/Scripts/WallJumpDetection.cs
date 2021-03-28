using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpDetection : MonoBehaviour
{
    public ReMovement ReMove;
    public BoxCollider WallLeft;
    public BoxCollider WallRight;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLeft()
    {
        Debug.Log("Left");
        //WallLeft.enabled = !WallLeft;
    }

    public void UpdateRight()
    {
        Debug.Log("Right");
        //WallRight.enabled = !WallRight;
    }

    

}
