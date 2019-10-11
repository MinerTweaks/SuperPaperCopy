using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UserInput : MonoBehaviour {

    public List<PaperController> selectedUnits;

    public PaperController target;

    public float ignoreMovementThreshold = 0.15f;

    private void Update () {
        Vector3 moveDirection = new Vector3( Input.GetAxis( "Horizontal" ) , 0.0f , Input.GetAxis( "Vertical" ) );
        // add in a deadzone
        if(moveDirection.magnitude >= ignoreMovementThreshold || moveDirection.magnitude == 0)
            target.SetMoveDirection( moveDirection );   

        if ( Input.GetKeyUp( KeyCode.Space ) ) {
            target.Jump();
        }
    }

    private void GetLeftClick () {
        // basic left click will add a unit to the selection

        if ( Input.GetMouseButtonUp( 0 ) ) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            if ( Physics.Raycast( ray , out hit , 10000.0f ) ) {
                //if ( hit.collider.GetComponentInChildren<ISelectable>() != null ) {

                    Debug.Log( "You selected the " + hit.transform.name );

                    //hit.collider.GetComponentInChildren<ISelectable>().OnSelect();

                    /*if ( selectedUnits.Contains( hit.collider.GetComponent<Character>() ) == false )
                        selectedUnits.Add( hit.collider.GetComponent<Character>() );*/
               // }
            }
        }


    }

    private void GetRightClick () {
        if ( Input.GetMouseButtonUp( 1 ) ) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            if ( Physics.Raycast( ray , out hit , 10000.0f ) ) {
                foreach ( var item in selectedUnits ) {
                    Debug.DrawLine( hit.point , item.transform.position , Color.black , 5.0f );
                    //item.SetCommand( new TravelCommand( item , hit.point ) );
                    //item.SetDestination( hit.point );
                }

                //Debug.Log( "You selected the " + hit.transform.name );
            }
        }
    }

    private Vector3 GetMousePositionHit () {
        if ( Input.GetKeyUp( KeyCode.Z ) ) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            if ( Physics.Raycast( ray , out hit , 500.0f ) ) {
                return hit.point;
            }
        }
        return new Vector3();
    }


    private Vector3 GetMousePosition () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        if ( Physics.Raycast( ray , out hit , 500.0f ) ) {
            return hit.point;
        }
        return new Vector3();
        /*
        Vector3 pos = Camera.main.ScreenToWorldPoint( Input.mousePosition );

        pos.z = 0;

        return pos;*/
    }


    private PaperController GetActorUnderMouse () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        
        if ( Physics.Raycast( ray , out hit , 10000.0f ) ) {
            if ( hit.transform.GetComponent<Collider>() != null && hit.transform.parent.GetComponent<PaperController>() ) 
                return hit.transform.parent.GetComponent<PaperController>();
        }

        return null;
    }
}
