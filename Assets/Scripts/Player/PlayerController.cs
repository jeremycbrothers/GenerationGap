using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// The player controller class will be responsible for handling all functionality
/// where input from the player results in some action preformed by the the possesed character
///</summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    // Required Components
    private Rigidbody2D rb;
    private InputManager input;
    private Animator anim;
    private Collider2D collider2d;

    // Serialized Components/variables (Usable in editor)
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundMask;

    private VelocityState state;
    private BoxCollider2D hitBox;
    private int activeFrameCount = 1;

    public VelocityState getState() { return state; }
    public void updateState(VelocityState state) { this.state = state; }

    // Allows the player to jump.
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = VelocityState.jumping;
    }

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = VelocityState.idle;
    }

    // Update is called a fixed amount per second
    void Update()
    {
        // Allow movement if the player is not recovering from damage.
        if(state != VelocityState.hurt)
        {
            Movement();
        }

        UpdateAnimationState();

        if(Input.GetButtonDown("Fire1"))
        {
            state = VelocityState.attacking;
        }
        
        anim.SetInteger("state", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.gameObject.GetComponent<Enemy>();
        enemy.SetDeathTrigger();
    }

    // Move the character when input is recieved from the player
    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        // Moving left
        if(hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        // Moving right
        else if(hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        // Jumping
        if(Input.GetButtonDown("Jump") && collider2d.IsTouchingLayers(groundMask))
        {
            Jump();
        }
    }

    // UpdateAnimationState updates the velocity state which allows or disallows
    // certian actions to be preformed by the player.
    private void UpdateAnimationState()
    {
        if(state == VelocityState.jumping)
        {
            if(rb.velocity.y < .1f) 
            {
                state = VelocityState.falling;
            }
        }

        else if(state == VelocityState.falling)
        {
            if(collider2d.IsTouchingLayers(groundMask))
            {
                state = VelocityState.idle;
            }
        }

        else if(state == VelocityState.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = VelocityState.idle;
            }
        }

        else if(Mathf.Abs(rb.velocity.x) > 0)
        {
            state = VelocityState.walking;
        }

        else
        {
            state = VelocityState.idle;
        }
    }

    private void AttackBegin()
    {
        Debug.Log("Attack Begin");
        // Here we need to create a box collider
        // Then resize and relocate the collider such that it is in the 
        // position of the characters "weapon"
        // Then we need to check if it collides with an enemy
        // every frame untill attack end is called
        // This is asuming the player is using a melee weapon
        hitBox = gameObject.AddComponent<BoxCollider2D>();

        if(hitBox)
        {
            hitBox.isTrigger = true;
            hitBox.offset = new Vector2(.9f, -.15f);
            hitBox.size = new Vector2(1.05f, 0.5f);
        }
    }

    private void AttackEnd()
    {
        if(hitBox)
            Destroy(hitBox);
        // remove the box collider
        Debug.Log("Attack End");
        activeFrameCount = 0;
    }
}
