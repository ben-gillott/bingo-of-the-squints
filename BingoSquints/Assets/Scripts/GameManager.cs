using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int currentBees;
    public int beesToWin;

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
    }

    public void WinState()
    {
        if (currentBees == beesToWin)
        {
            Debug.Log("Weeeenar");
        }
    }
}
