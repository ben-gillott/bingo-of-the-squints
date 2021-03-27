using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Beat", 0, 0.68f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Beat()
    {
        Debug.Log("Beat");
    }
}
