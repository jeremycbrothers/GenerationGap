using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skunk : Enemy
{
    private Projectile projectile;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check that the collision is with the player and that it
        // ignores any collider that is considered a trigger.
        if(collider.gameObject.tag == "Player")
        {
            if(!collider.isTrigger)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        Projectile projectile = gameObject.AddComponent<Projectile>();
        projectile.CalculateMove();
    }
}