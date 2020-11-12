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
    [Tooltip("Used to calculate the amount of push back happens when the player is damaged.")]
    [SerializeField] private float hurtForce = 1f;

    [Tooltip("Players default health")]
    [SerializeField] private int health;

    [Tooltip("Player health UI element")]
    [SerializeField] private Text healthAmount;

    /**
        The rigid body is used in determining whether or not the player is infornt or behind an enemy.!--
        GetRBPosition allows another script to access this information.!--
        @return rb (RigidBody2D) - physics object used to get the players location
    */
    public Vector2 GetRBPosition() { return rb.position; }

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
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
                enemy.SetDeathTrigger(); // This kills the enemy. It also assumes that all enemies have one health.
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
        // Uncomment when UI is ready.
        // healthAmount.text = health.ToString();
        
        // Reset the level if player dies. 
        if(health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
