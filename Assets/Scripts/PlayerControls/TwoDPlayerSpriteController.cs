using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 
/// </summary>
public class TwoDPlayerSpriteController : MonoBehaviour
{
    [Header("Required References")]
    [Tooltip("The player movement script that moves the player controller that this script has the visual representation for")]
    public PlayerMovement playerMovementScript;
    [Tooltip("The sprite renderer which renders the 2D version of the player character")]
    public SpriteRenderer characterSpriteRenderer;
    [Tooltip("The Animator which controls the 2D player's animations")]
    public Animator spriteAnimationController;

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
        if (spriteAnimationController != null)
        {
            SetAnimationState();
        }
    }

    /// <summary>
    /// Aligns the visual representation with the direction the player controller is moving.
    /// </summary>
    void AlignRepresentation()
    {
        Vector2 twoDMoveInput = moveInput.ReadValue<Vector2>();

        Vector3 movementDirection = new Vector3(twoDMoveInput.x, 0, twoDMoveInput.y).normalized;
        if (movementDirection.x > 0)
        {
            characterSpriteRenderer.flipX = false;
        }
        else if (movementDirection.x < 0)
        {
            characterSpriteRenderer.flipX = true;
        }
    }

    /// <summary>
    /// Sets the animation parameters of the sprite animation controller according to how the player is moving
    /// </summary>
    void SetAnimationState()
    {
        Vector2 twoDMoveInput = moveInput.ReadValue<Vector2>();

        if(twoDMoveInput.magnitude != 0)
        {
            spriteAnimationController.SetBool("IsIdle", false);
        }
        else
        {
            spriteAnimationController.SetBool("IsIdle", true);
        }
    }
}
