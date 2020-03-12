using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour {

    public GameObject text;
    public GameObject timeText;
    public static bool GameIsOver = false;
    private void Start()
    {
        text.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject hitObj = collider.gameObject;
 
            if (hitObj.tag == "Player")
            {
            Time.timeScale = 0f;
                text.SetActive(true);
            timeText.SetActive(false);
                //StartCoroutine("WaitforSec");
                // transform.parent.gameObject.AddComponent<GameOverScript>();

            }
        }
    
/*
    IEnumerator WaitforSec()
    {
        yield return new WaitForSeconds(5);
        Destroy(text);
        Destroy(gameObject);
    }
    */
}
