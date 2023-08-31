using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 
/// </summary>
public class ThreeDPlayerRepresentation : MonoBehaviour
{
    [Header("Required References")]
    [Tooltip("The player movement script that moves the player controller that this script has the visual representation for")]
    public PlayerMovement playerMovementScript;
    [Tooltip("The Animator which controls the 3D player's animations")]
    public Animator modelAnimationController;

    // Move input taken from the player movement script and used here to determine which way the representation should face.
    InputAction moveInput;

    // Start is called before the first frame update
    void Start()
    {
        moveInput = playerMovementScript.GetMove();
    }

    // Update is called once per frame
    void Update()
    {
        AlignRepresentation();
        if (modelAnimationController != null)
        {
            SetAnimationState();
        }
    }

    /// <summary>
    /// Aligns the visual representation with the direction the player controller is moving.
    /// </summary>
    void AlignRepresentation()
    {
        Vector2 moveDirection = moveInput.ReadValue<Vector2>();

        Vector3 movementDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
        if (movementDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y + playerMovementScript.transform.rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;
        }
    }

    /// <summary>
    /// Sets the animation parameters of the sprite animation controller according to how the player is moving
    /// </summary>
    void SetAnimationState()
    {
        Vector2 twoDMoveInput = moveInput.ReadValue<Vector2>();

        if (twoDMoveInput.magnitude != 0)
        {
            modelAnimationController.SetBool("IsIdle", false);
        }
        else
        {
            modelAnimationController.SetBool("IsIdle", true);
        }
    }
}
