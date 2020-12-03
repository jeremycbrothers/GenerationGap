using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //reference to Joe, whose position is used to calculate camera position
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("CuteJoe");
    }

    void Update()
    {
        //camera follows Joe on x axis
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }
}
