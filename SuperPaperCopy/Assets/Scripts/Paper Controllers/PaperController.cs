using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A base implementation of a super paper mario style sprite view using the unity's built in character controller. Input is handled in the UserInput class , to seperate the
/// input allowing for enemies to use it.
/// SIDE NOTE - This will only work with a default camera angle, which really sucks and I am looking for a way to fix this.
/// </summary>
public class PaperController : MonoBehaviour {
    [Header( "References" )]
    [SerializeField] private CharacterController controller;
    [SerializeField] private SpriteRenderer sprRenderer;

    [Header( "Right and left sprites" )]
    public Sprite facingRightSprite;
    public Sprite facingLeftSprite;

    [Header( "Character controller stats" )]
    public float gravity = 20.0f;
    public float speed = 2;
    public float jumpForce = 4;


    [Header( "Sprite rotation" )]
    [Tooltip( "The speed at which the sprite rotates" )] public float rotateSpeed = 120;
    [Tooltip( "Max angle the sprite can rotate to" )] public float angle = 45;

    private Vector3 moveDirection;
    private Quaternion targetRotation;
    private Vector3 localDirection;

    /// <summary>
    /// Adds in menu item to create sprite 3d. Sprites will still need to be set in the inspector!
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem( "GameObject/3D Object/Sprite 3D" , false , 10 )]
    static void CreateCustomGameObject ( MenuCommand menuCommand ) {
        GameObject go = new GameObject( "Sprite3D" );
        GameObjectUtility.SetParentAndAlign( go , menuCommand.context as GameObject );

        PaperController pc = go.AddComponent<PaperController>();
        pc.controller = go.AddComponent<CharacterController>();

        GameObject spr = new GameObject( "Sprite" );
        spr.transform.SetParent( go.transform , false );
        pc.sprRenderer = spr.AddComponent<SpriteRenderer>();
        pc.sprRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        pc.sprRenderer.receiveShadows = true;

        Debug.LogWarning("Set the sprites material to the SpriteLit material in materials folder!");

        Undo.RegisterCreatedObjectUndo( go , "Create " + go.name );
        Selection.activeObject = go;
    }


    private void Update () {
        // transform.GetChild( 0 ).transform.rotation = Quaternion.Euler(new Vector3( 0,Camera.main.transform.rotation.eulerAngles.y,0 )); Ignore me attempting to align with camera view using second gameobject

        Movement();
        SetSpriteState();
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
                targetRotation = Quaternion.LookRotation( moveDirection , transform.up );
            } else if ( moveDirection.x < 0 ) {
                // if facing left without this bit , the target direction won't be oriented properly
                targetRotation = Quaternion.LookRotation( -moveDirection , transform.up );
            }
        }
    }

    /// <summary>
    /// Set the y value of move direction to perform a jump but only if we are grounded
    /// </summary>
    public void Jump () {
        if ( controller.isGrounded == true )
            moveDirection.y = jumpForce;
    }


    #endregion character controller
    /// <summary>
    /// Set the sprite on the spriterenderer based on the x movement
    /// </summary>
    private void SetSpriteState () {
        if ( moveDirection.x != 0 )
            sprRenderer.sprite = moveDirection.x > 0 ? facingRightSprite : facingLeftSprite;
    }

    /// <summary>
    /// Rotates the sprite in the direction of character movement - This is the important part of the code, the rest is pretty irrelevant.
    /// This takes the targetRotation variable and rotates the localDirection towards it , not exceeding the max angle , then sets the sprites rotation to the value recieved.
    /// </summary>
    private void RotateSprite () {
        // rotate the vector3 towards the target direction with the angle clamping the max rotation
        localDirection = Vector3.RotateTowards( Vector3.forward , targetRotation * -transform.right , Mathf.Deg2Rad * angle , float.MaxValue );
        Debug.DrawRay(transform.position , -Camera.main.transform.right);
        // apply the rotation
        sprRenderer.transform.localRotation = Quaternion.RotateTowards( sprRenderer.transform.localRotation , Quaternion.LookRotation( localDirection ) , rotateSpeed * Time.deltaTime );
    }
}