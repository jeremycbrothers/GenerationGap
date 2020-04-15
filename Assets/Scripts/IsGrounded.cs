using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This could be moved into somekind of playerCharacter or playerController class
///</summary>
[RequireComponent(typeof(Collider2D))]
public class IsGrounded : MonoBehaviour
{
    public bool isGrounded { get; private set; }
    public LayerMask groundMask;

    public event EventHandler EntityLand;

    private Collider2D[] colliders;

    private void Awake()
    {
        colliders = GetComponents<Collider2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
    }

    private bool CheckIfGrounded()
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider.IsTouchingLayers(groundMask))
            {
                if (!isGrounded) OnEntityLand(new EventArgs());
                return true;
            }
        }
        return false;
    }

    private void OnEntityLand(EventArgs e)
    {
        EventHandler handler = EntityLand;
        if (handler != null)
        {
            handler(this, e);
        }
    }
}
