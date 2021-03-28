using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VinManager : MonoBehaviour
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
    public void NextVin()
    {
        MenuEmpts[0].active = false;
        MenuEmpts[1].active = true;
    }

    public void BacktoMenu()
    {
        MenuEmpts[1].active = false;
        MenuEmpts[0].active = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

}
