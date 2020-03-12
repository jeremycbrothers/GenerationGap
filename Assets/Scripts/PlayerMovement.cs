using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputManager))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 10f;
    public IsGrounded isGrounded;
    public bool hasDoubleJump = true;

    public Health health;

    public float normalGravity = 1f, fallingGravity = 3f;

    private bool facingRight = true;
    private InputManager inputManager;
    private Rigidbody2D rBody;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rBody = GetComponent<Rigidbody2D>();

        isGrounded.EntityLand += OnEntityLand;
        health.Die += OnDie;
    }

    private void OnDestroy()
    {
        isGrounded.EntityLand -= OnEntityLand;
        health.Die -= OnDie;
    }

    private void FixedUpdate()
    {
        InputState input = inputManager.input;

        Vector2 newVelocity = new Vector2(input.horizontalMovement * speed, rBody.velocity.y);

        if ((isGrounded.isGrounded || hasDoubleJump) && input.jump)
        {
            if (!isGrounded.isGrounded) hasDoubleJump = false;

            newVelocity = new Vector2(newVelocity.x, jumpForce);
            input.jump = false;
        }

        if (!isGrounded.isGrounded && (!input.jumpHold || newVelocity.y < 0))
        {
            rBody.gravityScale = fallingGravity;
        }
        else
        {
            rBody.gravityScale = normalGravity;
        }

        if ((facingRight && newVelocity.x < 0) || (!facingRight && newVelocity.x > 0)) Flip();

        rBody.velocity = newVelocity;

    }

    private void Flip()
    {
        //Track current direction the sprite is facing.
        facingRight = !facingRight;

        transform.Rotate(new Vector3(0, 180, 0));
    }

    private void OnEntityLand(object sender, EventArgs e)
    {
        hasDoubleJump = true;
    }

    public void OnDie(object sender, EventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
