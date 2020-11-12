using UnityEngine;

/// <summary>
/// The Skunk class is a child of the enemy class. 
/// It has the added ability of movement and attacking the
/// player if they get too close.
/// </summary>
public class Skunk : Enemy
{
    // Serialized fields. (Private memebers accessable by the editor.)
    [Tooltip("This is where your projectile prefab should be inserted")]
    [SerializeField] private SkunkProjectile skunkProjectile;
   
    [Tooltip("A transform that should be a child component of the skunk")]
    [SerializeField] private Transform projectileSpawnPoint;

    

    // Private memebers
    private float savedMoveSpeed;
    private RaycastHit2D hit;
    private Vector2 direction = new Vector2(2, -1); //positive direction for raycasting
    private float rayLen = 2.5f; //length of the ray used to detect edges

    /**
        Get facing right is used to inform a caller if this sprite is facing in a x positive direction
        @return facingRight (bool) - return true if facing right. Otherwise false.
    */
    public bool GetFacingRight() { return facingRight; } 

    // Sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check that the collision is with the player and that it
        // ignores any collider that is considered a trigger.
        if(collider.gameObject.tag == "Player")
        {
            // The player character will multiple box colliders,
            // but we only want to interact with the players hurt box here.
            if(!collider.isTrigger)
            {
                moveSpeed = 0;
                
                // If true, the player is further right on the x axis then the skunk. Else, it is to the left.
                if(playerCharacter.transform.position.x > transform.position.x)
                {
                    // If true, the skunk is facing the player. Else, it's facing away.
                    if(transform.right.x == transform.localScale.x)
                    {
                        Flip();
                        facingRight = false;
                    }

                    else 
                    {
                        facingRight = false; // In case it was previously true
                    }
                }

                else
                {
                    // If true the skunk is facing the player. Else, it's facing away.
                    if(transform.right.x != transform.localScale.x)
                    {
                        Flip();
                        facingRight = true;  
                    }  

                    else
                    {
                        facingRight = true;
                    }
                }
                Instantiate(skunkProjectile, new Vector3(projectileSpawnPoint.position.x, projectileSpawnPoint.position.y, projectileSpawnPoint.position.z), new Quaternion(0.0f,0.0f,0.0f,0.0f));
            }
        }
    }

    // Sent when another object leaves a trigger collider attached to this object (2D physics only).
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if(!collider.isTrigger)
            {
                moveSpeed = savedMoveSpeed;
                // This makes sure that the skunk does move in the wrong direction
                // after the player has left its attack range
                if(moveSpeed > 0.0f && transform.localScale.x < 0.0f || 
                   moveSpeed < 0.0f && transform.localScale.x > 0.0f)
                {
                    moveSpeed = -moveSpeed;
                }                
            }
        }
    }

    // Called once per frame
    private void Update()
    {
        rigidBody2D.velocity = new Vector2(moveSpeed, rigidBody2D.velocity.y);
        // It won't try and update the movement speed if the skunk is stopped.
        if(moveSpeed != 0.0f)
        {
            savedMoveSpeed = moveSpeed;
        }

        DetectEdge();
       
    }

    //Casts ray directed in front and beneath skunk to detect when there is no ground tile in front of it.
    //If the ray does not collide with a ground tile, skunk turns around
    private void DetectEdge()
    {
        if (transform.localScale.x < 0f) //skunk is facing left, project ray to the left
        {
            //Debug.DrawRay(transform.position, (new Vector2(-direction.x, direction.y)) * rayLen, Color.red);
            hit = Physics2D.Raycast(transform.position, new Vector2(-direction.x, direction.y), rayLen, LayerMask.GetMask("Ground"));
        }
        else //skunk is facing right, project ray to the right
        {
            //Debug.DrawRay(transform.position, direction * rayLen, Color.red);
            hit = Physics2D.Raycast(transform.position, direction, rayLen, LayerMask.GetMask("Ground"));
        }

        if (hit.collider == null) //no ground detected, skunk turns to move in opposite direction
        {
            Flip();
            moveSpeed = -moveSpeed;
        }
    }
}

