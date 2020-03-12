using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour {

    public LayerMask sensingMask;
    public Enemy reportTo;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Check if the object is on a layer in the layer mask
        if (sensingMask == (sensingMask | (1 << collision.gameObject.layer)))
            reportTo.ObjectInSensor(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the object is on a layer in the layer mask
        if (sensingMask == (sensingMask | (1 << collision.gameObject.layer)))
            reportTo.ObjectEnteredSensor(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Check if the object is on a layer in the layer mask
        if (sensingMask == (sensingMask | (1 << collision.gameObject.layer)))
            reportTo.ObjectLeftSensor(collision.gameObject);
    }

}
