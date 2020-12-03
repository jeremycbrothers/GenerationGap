using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

///<summary>
/// The player controller class will be responsible for handling all functionality
/// where input from the player results in some action preformed by the the possesed character
///</summary>
[RequireComponent(typeof(Rigidbody2D),typeof(Animator))]
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
    private Animator anim;
    private CapsuleCollider2D capsuleCollider2d;

    // Private members
    private GameObject Camera;
    private CinemachineVirtualCamera VirtualCamera;
    private CinemachineFramingTransposer fComposer;
    private PlayerCharacter.VelocityState state;
    private BoxCollider2D hitBox;
    private bool canEndLevel = false;

    /**
        Gets the velocity state so that an animator component can make necessary updates.
        @return state (VelocityState) - the state machine used by an animator
    */
    public PlayerCharacter.VelocityState getState() { return state; }

    /**
        Used by an animator component to update the players animations
        @param state (VelocityState) - the new velocity state.
    */
    public void updateState(PlayerCharacter.VelocityState state) { this.state = state; }

    // Allows the player to jump.
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = PlayerCharacter.VelocityState.jumping;
    }

    /**
        If the player is allowed to crouch at a given time, the camera will move downwards in the y direction
        so that the player can peek below them and see if there is a place for them to jump down to safely.
        @param crouching (bool) - determines if the camera should move up or down when crouching or standing up.
    */
    private void toggleCrouch(bool crouching)
    {
        if(crouching)
        {
            state = PlayerCharacter.VelocityState.crouch; // Used for animation state changes
                
            fComposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            fComposer.m_ScreenY = 0.0f;
            
        }

        else
        {
            state = PlayerCharacter.VelocityState.idle; // Used for animation state changes
            fComposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            fComposer.m_ScreenY = 0.5f;
        }
    }

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerCharacter.VelocityState.idle;
        Camera = GameObject.Find("2DCamera(Clone)");
        VirtualCamera = Camera.GetComponent<CinemachineVirtualCamera>();
        fComposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
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
        if(state != PlayerCharacter.VelocityState.hurt)
        {
            Movement();
        }

        UpdateAnimationState();

        if(Input.GetButtonDown("Fire1") && state != PlayerCharacter.VelocityState.crouch)
        {
            state = PlayerCharacter.VelocityState.attacking;
        }
        
        if(Input.GetButtonDown("Crouch") && state == PlayerCharacter.VelocityState.idle)
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
        if (state != PlayerCharacter.VelocityState.crouch) //Can't move while crouched
        {

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = 12;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 5;
            }

            float hDirection = Input.GetAxis("Horizontal");

            // Moving left
            if (hDirection < 0)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.localScale = new Vector2(-1, 1);
            }

            // Moving right
            else if (hDirection > 0)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.localScale = new Vector2(1, 1);
            }

            // Jumping
            if (Input.GetButtonDown("Jump") && capsuleCollider2d.IsTouchingLayers(groundMask) && state != PlayerCharacter.VelocityState.crouch)
            {
                Jump();
            }

        }
    }

    // UpdateAnimationState updates the velocity state which allows or disallows
    // certian actions to be preformed by the player.
    private void UpdateAnimationState()
    {
        switch(state)
        {
            case PlayerCharacter.VelocityState.idle:
                if(Mathf.Abs(rb.velocity.x) > 0)
                {
                    state = PlayerCharacter.VelocityState.walking;
                }
            break;
            case PlayerCharacter.VelocityState.walking:
                if(Mathf.Abs(rb.velocity.x) < .1f)
                {
                    state = PlayerCharacter.VelocityState.idle;
                }
            break;
            case PlayerCharacter.VelocityState.jumping:
                if(rb.velocity.y < 0.1f)
                {
                    state = PlayerCharacter.VelocityState.falling;
                }
            break;
            case PlayerCharacter.VelocityState.falling:
                if(capsuleCollider2d.IsTouchingLayers(groundMask))
                {
                    state = PlayerCharacter.VelocityState.idle;
                }
            break;
            case PlayerCharacter.VelocityState.hurt:
                if(Mathf.Abs(rb.velocity.x) < .1f)
                {
                    state = PlayerCharacter.VelocityState.idle;
                }
            break;
            case PlayerCharacter.VelocityState.crouch:
                if(Input.GetButtonUp("Crouch"))
                {
                    state = PlayerCharacter.VelocityState.crouch;
                }
            break;
            default:
                state = PlayerCharacter.VelocityState.idle;
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
