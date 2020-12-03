using System.Collections;
using UnityEngine;

/// <summary>
/// The projectile class is the base class for all projectiles.
/// It is responsible for when the projectile spawns and when it is destroyed.
/// Child classes should always be attached to a spcific projectile prefab.
/// </summary>
public class Projectile : MonoBehaviour
{
    // Serialized feilds. (protected memebers accessable by the editor.)
    [Tooltip("This field comes from the projectile base class.")]
    [SerializeField]protected Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When player collides with trigger, they take damage, and projectile disappears
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerCharacter.PlayerCharacter>().TakeDamage();
            Destroy(this.gameObject);
        }
    }
}