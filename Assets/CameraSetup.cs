using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSetup : MonoBehaviour
{
    private Camera camMain;
    private CinemachineVirtualCamera cam; //ref to cinemachine camera

    // Start is called before the first frame update
    void Start()
    {
        camMain = Camera.main; //ref to main camera
        cam = GetComponent<CinemachineVirtualCamera>(); 
        cam.Follow = GameObject.Find("CuteJoe(Clone)").transform; //set cinemachine cam to follow player

        camMain.gameObject.AddComponent(typeof(CinemachineBrain));
    }
}
