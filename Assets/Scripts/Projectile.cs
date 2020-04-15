using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This does not need to be an enemy
public class Projectile : Enemy
{
    public Vector3 velocity;
    public float angleOffset = -90f;
    public float vert;
    public float horiz;

    private void Start()
    {
        float deg = Mathf.Atan2(velocity.normalized.y, velocity.normalized.x) * 180 / Mathf.PI;

        transform.localEulerAngles = new Vector3(0, 0, deg + angleOffset);
    }

    public void CalculateMove()
    {
        vert = velocity.y;
        horiz = velocity.x;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //base.OnTriggerEnter2D(collision);

        Die();
    }

    public void Die()
    {
       // base.Die();

        Destroy(gameObject);
    }
}
