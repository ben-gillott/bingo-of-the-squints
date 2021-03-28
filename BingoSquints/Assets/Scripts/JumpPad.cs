using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float JumpPower;
    Vector3 JumpPadV3 = new Vector3(0, 5, 0);
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        other.attachedRigidbody.AddForce(JumpPadV3 * JumpPower, ForceMode.VelocityChange);
    }
}
