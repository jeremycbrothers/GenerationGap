using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the base class for enemy behavior that should be applicable to all enemies in the game.
/// This class handles basic enemy movement that can be refined in child classes if desired.
///</summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(CapsuleCollider2D))]
public class Enemy : MonoBehaviour
{
    // Required components
    private Rigidbody2D rigidBody2D;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;

    // Seralized fields
    // Increases speed by the factor set in the editor.
    [SerializeField] private float movementMultiplier = 1f;

    protected Vector3 movement;

    public void SetDeathTrigger()
    {
        animator.SetTrigger("death");
    }

    // Awake is called when the script instance is being loaded.
    public void Awake() 
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
}
