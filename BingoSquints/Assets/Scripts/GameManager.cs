using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int currentBees;
    public int beesToWin;

    public GameObject[] BeeArray;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WinState();
    }

    public void AddBee()
    {
        currentBees++;
        BeeArray[currentBees - 1].active = true;
    }

    public void WinState()
    {
        if (currentBees == beesToWin)
        {
            Debug.Log("Weeeenar");
        }
    }
}
