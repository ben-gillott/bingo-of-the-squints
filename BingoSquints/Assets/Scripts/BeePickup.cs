using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePickup : MonoBehaviour, IInteract
{
    public GameManager GM;

    public void InteractFunction(GameObject _Instagator)
    {
        Debug.Log("PickUp");
        GM.AddBee();
        Destroy(gameObject);
        SoundManagerScript.PlaySound("unlockSound");
    }

    public void InteractTrigger(GameObject _Instagator)
    {
        InteractFunction(_Instagator);
    }
}
