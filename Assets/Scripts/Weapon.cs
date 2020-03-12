using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform target;
    public float attackRange;
    public int damage;
    public float lastAttackTime;
    public float attackDelay;
    public GameObject projectile;
    public float projectForce;

    private Vector3 MovingDirection = Vector3.left;    //initial movement direction

    // Start is called before the first frame update
    void Start()
    {


    }
    void UpdateMovement(){
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer > attackRange)
        {
            if (this.transform.position.x > 2.4f)
            {
                MovingDirection = Vector3.left;
                gameObject.GetComponent<SpriteRenderer>().flipX = false;

            }
            else if (this.transform.position.x < -2f)
            {
                MovingDirection = Vector3.right;
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            this.transform.Translate(MovingDirection * Time.smoothDeltaTime);
        } else { 
            if (Time.time > lastAttackTime + attackDelay)
            {
                if (gameObject.GetComponent<SpriteRenderer>().flipX == false)
                {
                    GameObject newProj = Instantiate(projectile, transform.position, transform.rotation);
                    newProj.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(90f, projectForce));
                    lastAttackTime = Time.time;
                    Debug.Log("This is the 90F");
                    

                }
                else if(gameObject.GetComponent<SpriteRenderer>().flipX == true)
                {
                    GameObject newProj = Instantiate(projectile, transform.position, transform.rotation);
                    newProj.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-90f, projectForce));
                    lastAttackTime = Time.time;
                    Debug.Log("This is the -90F");

                }

            }
        }
    }

    

    // Update is called once per frame
   void Update()
    {
    UpdateMovement();
    /*
    //Check Distance
    if(distanceToPlayer < attackRange)
    {
    //Attack Cooldown
        if(Time.time > lastAttackTime + attackDelay)
        {

                GameObject newProj = Instantiate(projectile, transform.position, transform.rotation);
                newProj.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(90f, projectForce));
            lastAttackTime = Time.time;
        }
    }

*/


    }   

}