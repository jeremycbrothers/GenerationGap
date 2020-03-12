using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{

    public AudioSource Jumpsfx;
    public AudioSource Clicksfx;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jumpsfx.Play();
        if (Input.GetMouseButtonDown(0))
            Clicksfx.Play();
    }
}
