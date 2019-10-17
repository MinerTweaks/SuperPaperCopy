using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UserInput : MonoBehaviour {
    public PaperController target;

    public float ignoreMovementThreshold = 0.15f;

    private void Update () {
        Vector3 moveDirection = new Vector3( Input.GetAxis( "Horizontal" ) , 0.0f , Input.GetAxis( "Vertical" ) );
        // this adds a deadzone ignoring small inputs, but if no input is given , it passes that to prevent moving endlessly after moving once
        if(moveDirection.magnitude >= ignoreMovementThreshold || moveDirection.magnitude == 0)
            target.SetMoveDirection( moveDirection );   

        if ( Input.GetKeyUp( KeyCode.Space ) )
            target.Jump();
    }

}
