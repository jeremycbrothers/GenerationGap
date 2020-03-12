using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public InputState input { get; private set; }
    Animator anim;

    private void Awake()
    {
        input = new InputState();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        input.horizontalMovement = Input.GetAxis("Horizontal");

        //Using GetButtonDown so that these actions only happen once per button press.
        //Only setting to true otherwise if Update was called twice in a FixedUpdate and the button was pressed on the first update, the input would be set to false before it is handled.
        if (Input.GetAxis("Horizontal")!=0){
            anim.SetInteger("state", 1);
        }
        if (Input.GetAxis("Horizontal") == 0)
        {
            anim.SetInteger("state", 0);
        }
        if (Input.GetButton("Jump"))
        {
            anim.SetInteger("state", 3);
        }
        if (Input.GetButtonDown("Jump"))
        {
            input.jump = true;
            anim.SetInteger("state",3);
        }
        if (Input.GetButtonUp("Jump"))
        {
            anim.SetInteger("state", 0);
        }
        if (Input.GetButtonDown("Attack"))
        {
            input.attack = true;
            anim.SetInteger("state", 2);
        }
        if (Input.GetButtonUp("Attack"))
        {
            anim.SetInteger("state", 0);
        }
        input.jumpHold = Input.GetButton("Jump");
     
    }
}
