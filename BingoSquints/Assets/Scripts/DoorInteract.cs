using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorInteract : MonoBehaviour, IInteract
{
    public GameManager GM;

    public void InteractFunction(GameObject _Instagator)
    {
        if (GM.win)
        {
            SceneManager.LoadScene(3);
        }
        
    }

    public void InteractTrigger(GameObject _Instagator)
    {
        InteractFunction(_Instagator);
    }
}
