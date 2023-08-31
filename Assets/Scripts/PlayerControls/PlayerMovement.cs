using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Needed Game Object References")]
    [Tooltip("The player's camera in the scene")]
    public GameObject playerCamera;

    [Header("Following Game Objects")]
    [Tooltip("A list of FollowLikeChild scripts to call the following functions of after moving the player")]
    public List<FollowLikeChild> followers;

    [Header("Player Movement Settings")]
    [Tooltip("How fast the player moves along the X, Z plane")]
    public float moveSpeed = 10;
    [Tooltip("How much strength the player jumps with")]
    public float jumpStrength = 10.0f;
    [Tooltip("The force of gravity acting on the player")]
    public float gravity = 20.0f;
    protected Vector3 moveVector = Vector3.zero;

    private CharacterController characterController;

    [Header("Input Actions")]
    [Tooltip("The input action(s) that map to player movement")]
    public InputAction moveAction;
    [Tooltip("The input action(s) that map to player jumping")]
    public InputAction jumpAction;
    [Tooltip("The input action(s) that map to interacting with other objects")]
    public InputAction interactAction;

    [Header("Special Effects")]
    [Tooltip("The prefab of the jump effect to create when the player jumps")]
    public GameObject jumpEffect;

    protected Rigidbody playerRigidBody;

    bool jumping = false;

    /// <summary>
    /// Standard Unity function called whenever the attached gameobject becomes enabled
    /// </summary>
    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        interactAction.Enable();
    }

    /// <summary>
    /// Standard Unity function called whenever the attached gameobject becomes disabled
    /// </summary>
    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        interactAction.Disable();
    }

    /// <summary>
    /// Returns the move action input action
    /// </summary>
    /// <returns></returns>
    public InputAction GetMove()
    {
        return moveAction;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
        HandleMovement();
    }

    /// <summary>
    /// Handles the movement of the player using the character controller component and the new input system
    /// </summary>
    void HandleMovement()
    {
        // Handle the movement of the player using a character controller
        // and the new input system
        Vector2 moveDirection = moveAction.ReadValue<Vector2>();

        if (characterController.isGrounded)
        {
            jumping = false;

            moveVector = new Vector3(moveDirection.x, 0, moveDirection.y);
            moveVector = transform.TransformDirection(moveVector);
            moveVector *= moveSpeed;
        }
        // if not grounded (aka in air):
        else
        {
            moveVector = new Vector3(moveDirection.x * moveSpeed, moveVector.y, moveDirection.y * moveSpeed);
            moveVector = transform.TransformDirection(moveVector);
        }

        moveVector.y -= gravity * Time.deltaTime;


        // Handle the player jump action
        if (jumpAction.triggered && !jumping)
        {
            moveVector.y = jumpStrength;
            if (jumpEffect != null)
            {
                Instantiate(jumpEffect, transform.position, Quaternion.identity, null);
            }
            jumping = true;
        }

        // Handle interacting with the environment (i.e. initiating dialogue)
        if (interactAction.triggered)
        {
            DoInteraction();
        }

        // Move through the character controller
        characterController.Move(moveVector * Time.deltaTime);

        // Make all assigned followers do their following of the player now
        foreach (FollowLikeChild follower in followers)
        {
            follower.FollowParent();
        }
    }

    /// <summary>
    /// Makes the Y rotation of the player controller match the Y rotation of the player camera.
    /// </summary>
    void CameraRotation()
    {
        if(playerCamera == null)
        {
            Debug.LogError("No camera has been attached to the player script");
        }
        this.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, playerCamera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    #region Interaction
    [HideInInspector]
    [Tooltip("The interactable object that the player is currently able to interact with if any")]
    private Interactable activeInteractable;

    public void SetCurrentInteractable(Interactable newInteractable)
    {
        if (activeInteractable != null)
        {
            activeInteractable.HidePrompt();
        }
        activeInteractable = newInteractable;
        activeInteractable.ShowPrompt();
    }

    /// <summary>
    /// Sets active interactable if it matches the passed in interactable object
    /// </summary>
    /// <param name="unsetThisInteractable">The interactable object that should no longer be the active interactable</param>
    public void UnSetActiveInteractable(Interactable unsetThisInteractable)
    {
        if (unsetThisInteractable == activeInteractable)
        {
            activeInteractable.HidePrompt();
            activeInteractable = null;
        }
    }

    private void DoInteraction()
    {
        if (activeInteractable != null)
        {
            activeInteractable.DoInteraction();
        }
    }
    #endregion
}
