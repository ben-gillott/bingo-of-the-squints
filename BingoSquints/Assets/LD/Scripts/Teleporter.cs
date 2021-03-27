using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportDestination;

    bool able = false;
    GameObject thePlayer = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && able)
        {
            thePlayer.transform.position = teleportDestination.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject plyr = collision.gameObject;

            able = true;
            EnableTeleport(plyr);

            //show the player that they can now interact with the teleporter

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DisableTeleporter();
        }
    }

    void EnableTeleport(GameObject player)
    {
        thePlayer = player;
        able = true;
    }

    void DisableTeleporter()
    {
        thePlayer = null;
        able = false;
    }
}
