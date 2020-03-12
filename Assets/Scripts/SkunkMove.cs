using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SkunkMove : MonoBehaviour
{
    public Transform target;
    public float tarRange;

  

    private Vector3 MovingDirection = Vector3.left;    //initial movement direction
    void UpdateMovement()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer < tarRange)
        {
            if (this.transform.position.x > 2.4f)
            {
                MovingDirection = Vector3.left;
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (this.transform.position.x < -2f)
            {
                MovingDirection = Vector3.right;
                gameObject.GetComponent<SpriteRenderer>().flipX = false;

            }
            this.transform.Translate(MovingDirection * Time.smoothDeltaTime);
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }
}
