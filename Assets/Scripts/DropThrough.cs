
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropThrough : MonoBehaviour
{
    Collider2D c;

    void Start()
    {
        //Fetch the GameObject's Collider (make sure it has a Collider component)
        c = GetComponent<Collider2D>();
    }

    void Update()
    {
        //Toggle the Collider off when pressing s
        if (Input.GetKeyDown(KeyCode.S))
           c.enabled = false;
            
        //Toggle the Collider on when releasing s
        if (Input.GetKeyUp(KeyCode.S))
             c.enabled = true; 
    }
}
