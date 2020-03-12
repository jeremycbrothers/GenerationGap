using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Plats : MonoBehaviour
{
    public GameObject[] misc;
    public GameObject[] platform;
    bool powerUp;
    public Transform gen;
    public float dist;
    public float platwidth;
    float xaxis, yaxis;
    int power;
    private int num;
    public float buffer;
    // Start is called before the first frame update
    void Start()
    {

        //4 normal jump ratio 6 double

        //platwidth = platform.GetComponent<PolygonCollider2D>();
        for (int i = 0; i < platform.Length; i++)
        {
            transform.position = new Vector3(0, 0, 0);
            Instantiate(platform[i], transform.position, transform.rotation);
        }
    }
    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < platform.Length; i++)
        {
            platwidth = platform[i].GetComponent<BoxCollider2D>().size.x;
            if (transform.position.x <= gen.position.x)
            {

                if (powerUp == true)
                {
                    power = 6;
                }
                else { power = 3; }

                num = Random.Range(0, misc.Length);


                yaxis = (int)Random.Range(-power, power) + buffer;
                xaxis = transform.position.x + platwidth + dist;

                transform.position = new Vector3(xaxis, yaxis, 0);

                Instantiate(platform[i], transform.position, transform.rotation);

                Debug.Log(platwidth);
                Debug.Log(platform[i].GetComponent<BoxCollider2D>().size.x);
              
                // if (platform[i].GetComponent<BoxCollider2D>().size.x >= 5)
                if (platwidth >= 5)
                {
                    Instantiate(misc[num], transform.position, Quaternion.identity);
                }

                xaxis -= 3;
                yaxis -= 3;
                Instantiate(platform[i], transform.position, transform.rotation);
                xaxis++;
            }
        }

       }

   }

