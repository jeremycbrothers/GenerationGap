using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSilder : MonoBehaviour
{
    public Slider Volume;
    public AudioSource myMusic;

    private void Update()
    {
        myMusic.volume = Volume.value;
    }

}
