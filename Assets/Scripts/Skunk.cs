using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skunk : Enemy
{
    protected EnemyData enemyData;
    private float currentSpeedMultiplier;
    private bool canAttack = false;

    // OnCollisionEnter2D is called whenever this object collides with some Collision2D object
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateMovement(collision);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            movementMultiplier = 0;
            canAttack = true;
            PlayerCharacter player = collider.gameObject.GetComponent<PlayerCharacter>();
            if(player)
            {
                Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
                if(playerRB)
                {
                    if(movement.x < 0 && this.gameObject.GetComponent<Renderer>().bounds.center.x > playerRB.GetComponent<Renderer>().bounds.center.x)
                    {
                        gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    }

                    if(movement.x > 0 && this.gameObject.GetComponent<Renderer>().bounds.center.x < playerRB.GetComponent<Renderer>().bounds.center.x)
                    {
                        gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            movementMultiplier = currentSpeedMultiplier;
            canAttack = false;
            if(movement.x < 0 && gameObject.GetComponent<SpriteRenderer>().flipX == true)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            if(movement.x > 0 && gameObject.GetComponent<SpriteRenderer>().flipX == false)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.left;
        movementMultiplier = 1.75f;
        enemyData = new EnemyData(10f,10f); // Set default values for the enemy data. Can also be changed in editor.
        currentSpeedMultiplier = movementMultiplier;
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if(canAttack)
        {
            Attack();
        }
         Debug.Log("Skunk vector: " + movement + "\nSkunk position: " + this.gameObject.GetComponent<Renderer>().bounds.center + "\n");
    }

    private void Attack()
    {
        
    }
}
