using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject[] MenuEmpts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenCreds()
    {
        MenuEmpts[0].active = false;
        MenuEmpts[1].active = true;
        SoundManagerScript.PlaySound("menuScrollSound");
    }

    public void BacktoMenu()
    {
        SoundManagerScript.PlaySound("menuScrollSound");
        MenuEmpts[1].active = false;
        MenuEmpts[0].active = true;
    }

    public void StartGame()
    {
        SoundManagerScript.PlaySound("menuSelectSound");
        SceneManager.LoadScene(1);
    }

}
