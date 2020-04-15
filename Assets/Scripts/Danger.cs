using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This probably doesn't need to be a parent class
public class Danger : MonoBehaviour
{
    public bool damagesEnemies = false;
    public int contactDamage = 1;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Kill(collision.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Kill(collision.gameObject);
    }

    protected void Kill(GameObject gameObject)
    {
        Health toDamage = null;

        if (gameObject.tag == "Player")
        {
            toDamage = gameObject.GetComponent<Health>();
        }
        else if (damagesEnemies)
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                toDamage = gameObject.GetComponent<Health>();
            }
        }

        if (toDamage != null)
        {
            toDamage.health -= contactDamage;
            Debug.Log(name);
        }
    }
}
