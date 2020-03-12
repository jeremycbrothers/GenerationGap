using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the base class for enemy behavior that should be applicable to all enemies in the game.
/// This class handles basic enemy movement that can be refined in child classes if desired.
///</summary>
public class Enemy : MonoBehaviour
{
    protected Vector3 movement;
    public struct EnemyData
    {
        float health;
        float damage;

        public EnemyData(float health, float damage)
        {
            this.health = health;
            this.damage = damage;
        }
    }

    // Increases speed by the factor set in the editor.
    public float movementMultiplier = 1f;

    // FixedUpdate is called a fixed number of times per second.
    protected void FixedUpdate()
    {
        this.transform.Translate((movement * movementMultiplier) * Time.fixedDeltaTime);
    }

    ///<summary>
    /// UpdateMovement base functionality flips a sprite if it has collided with a Collision2D object.
    /// The sprite only flips if it collides with Collision2D objects with a tag of "VerticalCollider".
    /// This function, when called used in a child class, should be called within MonoBehavior's OnCollisionEnter2D(Collision2D collision) function.
    /// <param name="collision">A Collision2D object used that this class has collided with.</param>
    ///</summary>
    virtual protected void UpdateMovement(Collision2D collision)
    {
        // Get the tag set in the editor. If the collision's tag is 'VerticalCollider', or "Player", then determine which direction to flip.
        if(collision.gameObject.tag == "VerticalCollider")
        {
            // flip the sprite and velocity
            if(movement == Vector3.left)
            {
                movement = Vector3.right;
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                movement = Vector3.left;
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
}
