using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Streamline this , possibly add features, look into alternate method of calculating , see if there is a way to change camera angle?
/// </summary>
public class PaperController : MonoBehaviour {
    // references
    [Header( "References" )]
    public CharacterController controller;
    public Transform targetDirection;
    public SpriteRenderer sprRenderer;

    [Header( "Right and left sprites" )]
    public Sprite facingRightSprite;
    public Sprite facingLeftSprite;

    [Header( "Character controller stats" )]
    public float gravity = 20.0f;
    public float speed = 2;
    public float jumpForce = 4;

    private Vector3 moveDirection;

    [Header( "Sprite rotation" )]
    [Tooltip( "The speed at which the sprite rotates" )] public float rotateSpeed = 90;

    [Tooltip( "Max angle the sprite can rotate to" )] public float angle = 60;

    private Vector3 localDirection;

    /// <summary>
    /// Adds in menu item to create sprite 3d. Character controller and sprites will still need to be set in the inspector!
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem( "GameObject/3D Object/Sprite 3D" , false , 10 )]
    static void CreateCustomGameObject ( MenuCommand menuCommand ) {
        // Create a custom game object
        GameObject go = new GameObject( "Sprite3D" );
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign( go , menuCommand.context as GameObject );

        PaperController pc = go.AddComponent<PaperController>();
        pc.controller = go.AddComponent<CharacterController>();

        GameObject spr = new GameObject( "Sprite" );
        spr.transform.SetParent( go.transform , false );
        pc.sprRenderer = spr.AddComponent<SpriteRenderer>();

        pc.targetDirection = new GameObject("TargetDirection").transform;
        pc.targetDirection.transform.SetParent( go.transform , false );
        pc.targetDirection.transform.position = new Vector3( 0 , -0.5f , 0.1f ); 

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo( go , "Create " + go.name );
        Selection.activeObject = go;
    }


    private void Update () {
        Movement();
        RotateSprite();
    }

    #region Character controller

    /// <summary>
    /// Handles the movement of the character controller.
    /// </summary>
    private void Movement () {
        moveDirection.y = moveDirection.y - ( gravity * Time.deltaTime );
        controller.Move( moveDirection * Time.deltaTime );
    }

    /// <summary>
    /// Set the direction the character will move.
    /// </summary>
    /// <param name="dir"></param>
    public void SetMoveDirection ( Vector3 dir ) {
        if ( controller.isGrounded ) {
            moveDirection = dir.normalized * speed;

            // rotate the gameobject that indicates the target direction the object is facing
            if ( moveDirection.x > 0 ) {
                targetDirection.rotation = Quaternion.LookRotation( moveDirection , transform.up );
            } else if ( moveDirection.x < 0 ) {
                // if facing left without this bit , the target direction won't be oriented properly
                targetDirection.rotation = Quaternion.LookRotation( -moveDirection , transform.up );
            }
        }
    }

    /// <summary>
    /// Send a jump command if able
    /// </summary>
    public void Jump () {
        if ( controller.isGrounded == true )
            moveDirection.y = jumpForce;
    }


    #endregion character controller

    /// <summary>
    /// Rotates the sprite in the direction of character movement
    /// </summary>
    private void RotateSprite () {
        // might not be needed?
        // localDirection = -targetDirection.right;

        // set the sprite to face the right direction based off of where the character is moving
        if ( moveDirection.x != 0 )
            sprRenderer.sprite = moveDirection.x > 0 ? facingRightSprite : facingLeftSprite;

        // rotate the vector3 towards the target direction with the angle clamping the max rotation
        localDirection = Vector3.RotateTowards( Vector3.forward , -targetDirection.right , Mathf.Deg2Rad * angle , float.MaxValue );

        // apply the rotation
        sprRenderer.transform.localRotation = Quaternion.RotateTowards( sprRenderer.transform.localRotation , Quaternion.LookRotation( localDirection ) , rotateSpeed * Time.deltaTime );
    }
}