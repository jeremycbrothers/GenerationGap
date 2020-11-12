using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSpawner : MonoBehaviour
{

    [SerializeField] int spawnDistance;
    [SerializeField] Enemy[] spawned;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < spawnDistance; i++)
        {
            transform.position += new Vector3(1, 0, 0);
            //Debug.Log(difficultyRate(i));
            if (Random.Range(0f, 1f) < difficultyRate(i))
            {
                Enemy justSpawned = Instantiate(spawned[Random.Range(0, spawned.Length)], transform.position, Quaternion.identity);


                //RaycastHit hit;
                ////justSpawned.transform.position.y = 200f;
                
                //if (Physics.Raycast(justSpawned.transform.position, Vector3.down, out hit, 1000f))
                //{
                //    Debug.Log("hello");
                //    justSpawned.transform.position = hit.transform.position;
                //}


                //put the enemy on the ground via fast downward velocity
                justSpawned.rigidBody2D.velocity = new Vector2(0, -100f);
                //while (! justSpawned.IsGrounded()) //somehow move enemies to ground?
                //{
                //    justSpawned.transform.position += new Vector3(0, -1, 0);
                //}
            }
        }
    }

    private float difficultyRate(int x)
    {
        return ((float)x / ((float)x + 25f)) / 6;
    }
}
