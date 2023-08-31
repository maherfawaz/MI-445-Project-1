using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script to control animation of the falling board
/// </summary>
public class FallingPlankScript : MonoBehaviour
{

    public Animator boardAnimator;
    
    /// <summary>
    /// Starts the fall animation of the board
    /// </summary>
    public void StartFallAnimation()
    {
        boardAnimator.SetBool("Fall", true);
    }
}
