using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphinx : Enemy
{
    public float chargeDuration;


    private bool charging;
    private float chargeDirection,
                  chargeTimeLeft;

    public override void ObjectInSensor(GameObject obj)
    {
        base.ObjectInSensor(obj);

        if (obj.CompareTag("Player") && !charging)
        {
            charging = true;
            anim.SetBool("IsCharging", charging);

            float directionToPlayer = (obj.transform.position - transform.position).x;
            chargeDirection = Mathf.Sign(directionToPlayer);

            chargeTimeLeft = chargeDuration;
        }
    }

    public override void CalculateMove()
    {
        base.CalculateMove();

        if (charging)
        {
            chargeTimeLeft -= Time.deltaTime;

            if (chargeTimeLeft <= 0)
            {
                charging = false;
                anim.SetBool("IsCharging", charging);
            }
            else
            {
                horiz = speed * chargeDirection;
            }
        }
    }
}
