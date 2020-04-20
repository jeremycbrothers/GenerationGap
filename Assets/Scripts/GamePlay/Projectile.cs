using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This does not need to be an enemy
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    private Collider2D hitBox;

    // public Vector3 velocity;
    // public float angleOffset = -90f;
    // public float vert;
    // public float horiz;

    private void Start()
    {
        hitBox = GetComponent<Collider2D>();
        // float deg = Mathf.Atan2(velocity.normalized.y, velocity.normalized.x) * 180 / Mathf.PI;

        // transform.localEulerAngles = new Vector3(0, 0, deg + angleOffset);
    }

    public void CalculateMove()
    {
        // vert = velocity.y;
        // horiz = velocity.x;
    }

    private void Update()
    {
        transform.position += new Vector3(-0.01f, 0, 0);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        //base.OnTriggerEnter2D(collision);
        if(collision.gameObject.tag == "Player")
        {
            PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
            if(player)
            {
                Debug.Log("gottcha bitch!");
                player.TakeDamage();
                Destroy(this.gameObject);
            }
        }
        //Die();
    }

    public void Die()
    {
       // base.Die();

        Destroy(gameObject);
    }
}
