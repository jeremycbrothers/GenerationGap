using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float startingTime;
    public Text timerText;
    private bool Finished = false;
    
    // Start is called before the first frame update
    void Start()
    {
        startingTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Finished)
        {
            return;
        }
        float t = Time.time - startingTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");
        timerText.text = minutes + ":" + seconds;
       
    }

    public void Finish()
    {
        Finished = true;
        timerText.color = Color.red;
        
    }
}
