using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//add

public class Venus : Enemy
{
    [Tooltip("A reference to the player's character. It should be some Joe prefab.")]
    [SerializeField] private PlayerCharacter playerCharacter;

    private VelocityState state;

    // Sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check that the collision is with the player and that it
        // ignores any collider that is considered a trigger.
        if (collider.gameObject.tag == "Player")
        {
            // The player character will multiple box colliders,
            // but we only want to interact with the players hurt box here.
            if (!collider.isTrigger)
            {
                animator.SetBool("attacking", true);
            }
        }
    }

    // Sent when another object leaves a trigger collider attached to this object (2D physics only).
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (!collider.isTrigger)
            {
                animator.SetBool("attacking", false);
            }
        }
    }

    void Start()
    {

    }

    
    void Update()
    {
        // We need the player characters position and we get that via it's rigid body 2d component.
        // This is used in determining which way to flip the venus so that it can attack.
        Vector2 characterPosition2 = playerCharacter.GetRBPosition();
        Vector3 characterPosition3 = new Vector3(characterPosition2.x, characterPosition2.y, 0.0f);

        // If true, the player is further right on the x axis then the venus. Else, it is to the left.
        if (characterPosition3.x > transform.position.x)
        {
            // If true, the venus is facing the player. Else, it's facing away.
            if (transform.right.x != transform.localScale.x)
            {
                Flip();
            }
        }

        else
        {
            // If true the venus is facing the player. Else, it's facing away.
            if (transform.right.x == transform.localScale.x)
            {
                Flip();
            }
        }
    }
}
