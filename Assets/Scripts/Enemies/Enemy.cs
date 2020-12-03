using UnityEngine;

/// <summary>
/// This is the base class for enemy behavior that should be applicable to all enemies in the game.
/// This class handles basic enemy movement that can be refined in child classes if desired.
///</summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    // Required components
    protected Rigidbody2D rigidBody2D;
    protected Animator animator;
    protected bool facingRight = true;
    protected Vector3 movement;

    /**
        Used to update the enemies animator component.
    */
    public void SetDeathTrigger()
    {
        animator.SetTrigger("death");
    }

    /**
        Flip is a function that "flips" an enemies sprite
        by inverting it's x-scale.
    */
    protected void Flip()
    {
        transform.localScale = 
            new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); 
    }

    // Awake is called when the script instance is being loaded.
    public void Awake() 
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    // Flip the sprite if it collides with platforms/walls, and invert its speed.
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.tag == "Platforms" || collider.gameObject.tag == "Enemy")
        {
            Flip();
            moveSpeed = -moveSpeed;
        }
    }
}
