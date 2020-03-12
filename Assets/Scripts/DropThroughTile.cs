 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEnabled : MonoBehaviour
{
    Collider collider;

    void Start()
    {
        //Fetch the GameObject's Collider (make sure it has a Collider component)
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Toggle the Collider off when pressing s
            collider.enabled = false;

            //Output to console whether the Collider is on or not
            Debug.Log("Collider.enabled = " +collider.enabled);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            //Toggle the Collider on when releasing s
            collider.enabled = true;

            //Output to console whether the Collider is on or not
            Debug.Log("Collider.enabled = " + collider.enabled);
        }
    }
}