using UnityEngine;

///<summary>
/// The skunk projectile is a child class of projectile.
/// This class is responsible for calculating the trajectory of
/// the projectile.
/// This script should only be used with the SkunkProjectile prefab. 
/// Which should be at: Assets\Prefabs\Projectiles\SkunkProjectile.prefab 
///</summary>
public class SkunkProjectile : Projectile
{
    private int xVelocity = 2;
    private bool fireLeft;

    private Vector3 playerPosition;
    private Vector3 skunkPosition;

    // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    private  void Start()
    {
        xVelocity = 2;
        rb = GetComponent<Rigidbody2D>();
        PlayerCharacter pc = FindObjectOfType<PlayerCharacter>();
        playerPosition = pc.transform.position;
        Skunk skunk = FindObjectOfType<Skunk>();
        skunkPosition = skunk.transform.position;
        Destroy(gameObject, 2.0f);
    }
    
    // Update is called every frame, if the MonoBehaviour is enabled.
    private void Update()
    {
        Debug.Log(skunkPosition);
        if (playerPosition.x < skunkPosition.x)
        {
            rb.velocity = new Vector2(-2, 0);
        }
        else if(playerPosition.x > skunkPosition.x)
        {
            rb.velocity = new Vector2(2, 0);
        }
    } 
}
