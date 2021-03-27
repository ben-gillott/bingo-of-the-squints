using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerText : MonoBehaviour
{
    [SerializeField] GameObject textToDisplay = null;

    private void Start()
    {
        if (textToDisplay != null)
        {
            textToDisplay.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (textToDisplay != null)
            {
                textToDisplay.SetActive(true);
            }
            else
            {
                Debug.Log("There's no text assigned to this script, \nso there's nothing to display.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (textToDisplay != null)
            {
                textToDisplay.SetActive(false);
            }
        }
    }
}
