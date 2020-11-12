using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

///<summary>
/// The player controller class will be responsible for handling all functionality
/// where input from the player results in some action preformed by the the possesed character
///</summary>
[RequireComponent(typeof(Rigidbody2D),typeof(InputManager),typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{

    // Serialized Components/variables (Usable in editor)
    [Tooltip("Player movement speed")]
    [SerializeField] private float speed;

    [Tooltip("Jump force is relative to the gravity scale on the RigidBody2D component")]
    [SerializeField] private float jumpForce = 10f;

    [Tooltip("A layer mask used to determine which tiles are ground. Used to restrict the player to one jump.")]
    [SerializeField] private LayerMask groundMask;

    // Required Components
    private Rigidbody2D rb;
    private InputManager input;
    private Animator anim;
    private CapsuleCollider2D capsuleCollider2d;

    // Private members
    private VelocityState state;
    private BoxCollider2D hitBox;
    private bool canEndLevel = false;

    /**
        Gets the velocity state so that an animator component can make necessary updates.
        @return state (VelocityState) - the state machine used by an animator
    */
    public VelocityState getState() { return state; }

    /**
        Used by an animator component to update the players animations
        @param state (VelocityState) - the new velocity state.
    */
    public void updateState(VelocityState state) { this.state = state; }

    // Allows the player to jump.
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = VelocityState.jumping;
    }

    public void toggleCrouch(bool toggle)
    {       
        GameObject Camera = GameObject.Find("2DCamera(Clone)");
        CinemachineVirtualCamera VirtualCamera = null;
        if(Camera)
        {
            VirtualCamera = Camera.GetComponent<CinemachineVirtualCamera>();
        }

        if(toggle)
        {
            state = VelocityState.crouch;
            capsuleCollider2d.size = new Vector2(capsuleCollider2d.size.x, 1);
            capsuleCollider2d.offset = new Vector2(capsuleCollider2d.offset.x, -0.5f);
            if(VirtualCamera)
            {
                var composer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                composer.m_ScreenY = 0.0f;
            }
        }

        else
        {
            state = VelocityState.idle;
            capsuleCollider2d.size = new Vector2(capsuleCollider2d.size.x, 1.671995f);
            capsuleCollider2d.offset = new Vector2(capsuleCollider2d.offset.x, -0.1640023f);
            if(VirtualCamera)
            {
                var composer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                composer.m_ScreenY = 0.5f;
            }
        }
    }

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
        anim = GetComponent<Animator>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = VelocityState.idle;
    }

    // Update is called a fixed amount per second
    void Update()
    {
        if(canEndLevel)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        // Allow movement if the player is not recovering from damage.
        if(state != VelocityState.hurt)
        {
            Movement();
        }

        UpdateAnimationState();

        if(Input.GetButtonDown("Fire1") && state != VelocityState.crouch)
        {
            state = VelocityState.attacking;
        }
        
        if(Input.GetButtonDown("Crouch") && state == VelocityState.idle)
        {
            toggleCrouch(true);
        }

        else if(Input.GetButtonUp("Crouch"))
        {
            toggleCrouch(false);
        }

        if(Input.GetButtonDown("PlaceLadder"))
        {
            Debug.Log("Get the room that the player is \"in\"");

        }

        anim.SetInteger("state", (int)state);
    }

    // Sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Maybe I can use this information to get a refference to the room the player is in?
        // Maybe it would be better to have player information sent to the room?
        GameObject room = GameObject.Find(collider.gameObject.name);
        Debug.Log(room.name);
        // Check that only the player's weapon can kill an enemy.
        if(hitBox != null)
        {
            // Check that the hitbox is colliding with non triggers.
            if(!collider.isTrigger) {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                enemy.SetDeathTrigger(); // Assumes all enemies have one hp
            }
        }

        if(collider.gameObject.tag == "FinishLine")
        {
            canEndLevel = true;
        }

        if(collider.name == "DeathZone")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(canEndLevel)
        {
            canEndLevel = false;
        }
    }

    // Move the character when input is recieved from the player
    private void Movement()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 12;
        }
        
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5;
        }

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
        if(Input.GetButtonDown("Jump") && capsuleCollider2d.IsTouchingLayers(groundMask) && state != VelocityState.crouch)
        {
            Jump();
        }
    }

    // UpdateAnimationState updates the velocity state which allows or disallows
    // certian actions to be preformed by the player.
    private void UpdateAnimationState()
    {
        switch(state)
        {
            case VelocityState.idle:
                if(Mathf.Abs(rb.velocity.x) > 0)
                {
                    state = VelocityState.walking;
                }
            break;
            case VelocityState.walking:
                if(Mathf.Abs(rb.velocity.x) < .1f)
                {
                    state = VelocityState.idle;
                }
            break;
            case VelocityState.jumping:
                if(rb.velocity.y < 0.1f)
                {
                    state = VelocityState.falling;
                }
            break;
            case VelocityState.falling:
                if(capsuleCollider2d.IsTouchingLayers(groundMask))
                {
                    state = VelocityState.idle;
                }
            break;
            case VelocityState.hurt:
                if(Mathf.Abs(rb.velocity.x) < .1f)
                {
                    state = VelocityState.idle;
                }
            break;
            case VelocityState.crouch:
                if(Input.GetButtonUp("Crouch"))
                {
                    state = VelocityState.idle;
                }
            break;
            default:
                state = VelocityState.idle;
            break;
        }
    }

    private void AttackBegin()
    {
        // Create the hit box for the players weapon.
        hitBox = gameObject.AddComponent<BoxCollider2D>();

        if(hitBox)
        {
            // Note: it might be a good idea to replace this with a transform that is a 
            // child of the player
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
    }
}
