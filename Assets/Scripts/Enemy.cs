using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Danger
{

    public float speed = 4f;
    public bool flipOnAwake = false,
                canDie = true,
                autoStopHorizontal = true;

    protected bool facingRight = true,
                   dead = false;
    protected Rigidbody2D rBody;
    protected float horiz, vert, angVel;
    protected Animator anim;

    public void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (flipOnAwake) Flip();
    }

    public virtual void FixedUpdate()
    {
        if (rBody != null)
        {
            horiz = rBody.velocity.x;
            vert = rBody.velocity.y;
            angVel = rBody.angularVelocity;
        }
        else
        {
            horiz = 0;
            vert = 0;
            angVel = 0;
        }

        CalculateMove();

        if (rBody != null)
        {
            rBody.velocity = new Vector2(horiz, vert);
            rBody.angularVelocity = angVel;
        }
        else
        {
            transform.position += new Vector3(horiz, vert, 0) * Time.deltaTime;
            transform.localEulerAngles += new Vector3(0, 0, angVel * Time.deltaTime);
        }

        if (anim != null)
        {
            //Tell the animator how the enemy is moving.
            anim.SetFloat("Speed", Mathf.Abs(horiz));
            anim.SetFloat("vSpeed", vert);
        }

    }

    public virtual void CalculateMove()
    {
        if (autoStopHorizontal) horiz = 0;
    }

    public virtual void Flip()
    {
        //Track current direction the sprite is facing.
        facingRight = !facingRight;

        //Flip the sprite
        transform.Rotate(new Vector3(0, 180, 0));
    }

    public virtual void Die()
    {
        if (dead || !canDie) return;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        dead = true;

        Destroy(gameObject);
    }

    public virtual void ObjectInSensor(GameObject obj) { }
    public virtual void ObjectEnteredSensor(GameObject obj) { }
    public virtual void ObjectLeftSensor(GameObject obj) { }
}
