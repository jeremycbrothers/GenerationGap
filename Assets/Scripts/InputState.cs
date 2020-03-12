using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputState {

    public float horizontalMovement { get; set; }
    public bool jump { get; set; }
    public bool attack { get; set; }

    public bool jumpHold { get; set; }

}
