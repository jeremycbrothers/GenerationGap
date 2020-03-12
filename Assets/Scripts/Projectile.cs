using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Enemy
{
    public Vector3 velocity;
    public float angleOffset = -90f;

    private void Start()
    {
        float deg = Mathf.Atan2(velocity.normalized.y, velocity.normalized.x) * 180 / Mathf.PI;

        transform.localEulerAngles = new Vector3(0, 0, deg + angleOffset);
    }

    public override void CalculateMove()
    {
        vert = velocity.y;
        horiz = velocity.x;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        Die();
    }

    public override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}
