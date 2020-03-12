using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour
{
    public void FullscreenToggle()
    {
        if (Screen.fullScreen == true)
            Screen.fullScreen = false;
        else
            Screen.fullScreen = true;
    }
}
