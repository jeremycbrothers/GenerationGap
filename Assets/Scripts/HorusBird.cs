using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorusBird : Enemy
{
    public GameObject laserPrefab;
    public float shotFrequency = 3f, 
                 shotSpeed = 3f,
                 flapFrequency = .2f,
                 flapPower = 1f;

    private bool playerSeen;
    private Transform shotTarget;
    private float timeUntilShot = 0f,
                  timeUntilFlap = 0f;

    private float lowestVel = 0f;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        timeUntilShot -= Time.deltaTime;
        if (timeUntilShot <= 0f && playerSeen)
        {
            ShootLaser();
        }
    }

    public override void ObjectEnteredSensor(GameObject obj)
    {   
        base.ObjectEnteredSensor(obj);

        if (obj.CompareTag("Player"))
        {
            playerSeen = true;
            shotTarget = obj.transform;
        }
    }

    public override void ObjectLeftSensor(GameObject obj)
    {
        base.ObjectLeftSensor(obj);

        if (obj.CompareTag("Player"))
        {
            playerSeen = false;
            shotTarget = null;
        }
    }

    public override void CalculateMove()
    {
        base.CalculateMove();

        if (playerSeen)
        {
            float directionToPlayer = (shotTarget.position - transform.position).x;

            if ((directionToPlayer < 0 && !facingRight) || (directionToPlayer > 0 && facingRight))
                Flip();
        }

        horiz = (facingRight ? -.2f : .2f);

        if (rBody.velocity.y < lowestVel) lowestVel = rBody.velocity.y;

        timeUntilFlap -= Time.deltaTime;
        if (timeUntilFlap <= 0f)
        {
            vert = flapPower;
            timeUntilFlap = flapFrequency;
        }
    }

    private void ShootLaser()
    {
        Vector3 shotVelocity = (shotTarget.position - transform.position).normalized * shotSpeed;

        Projectile shot = Instantiate(laserPrefab, transform.position + shotVelocity.normalized, Quaternion.identity).GetComponent<Projectile>();
        shot.velocity = shotVelocity;

        timeUntilShot = shotFrequency;
        Debug.Log("pew");
    }
}
