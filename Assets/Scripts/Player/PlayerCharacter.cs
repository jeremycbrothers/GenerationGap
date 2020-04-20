using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerController), typeof(Collider2D))]
public class PlayerCharacter : MonoBehaviour
{
    // Required components
    private Collider2D collider2d;
    private Rigidbody2D rb;
    private PlayerController playerController;

    // Serialized components/variables
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private int health;
    [SerializeField] private Text healthAmount;

    public Collider2D GetCollider() { return collider2d; }

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    // Sent when an incoming collider makes contact with this object's collider (2D physics only)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // Get the velocity state of the player controller. If the player is falling,
            // "kill" the enemy. Otherwise take damage from the enemy.
            if(playerController.getState() == VelocityState.falling)
            {
                enemy.SetDeathTrigger(); // This kills the enemy.
                playerController.Jump();
            }

            else
            {
                // Determine which direction to push the player when they are in the hurt state
                if(collision.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                    playerController.updateState(VelocityState.hurt);
                    TakeDamage();
                } 
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                    playerController.updateState(VelocityState.hurt);
                    TakeDamage();
                }
            }
        }
    }

    public void TakeDamage()
    {
        health--;
//        healthAmount.text = health.ToString();
        if(health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
