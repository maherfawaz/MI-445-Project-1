using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
public class ThreeDToTwoDSwitchController : MonoBehaviour
{
    public static ThreeDToTwoDSwitchController instance;
    public enum VisualState { ThreeD, TwoD }

    [Header("Visual Representation Settings")]
    [Tooltip("The current visual state of the game, also sets all representations at game start and scene load")]
    public VisualState currentVisualState = VisualState.ThreeD;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        // Adds a delegate to the Scene Management to make the visuals change when a scene is loaded
        SceneManager.sceneLoaded += ChangeVisualOnSceneLoaded;
    }

    /// <summary>
    /// When loading a scene, change the visuals in that scene to match the current setting
    /// </summary>
    /// <param name="scene">The scene that was loaded</param>
    /// <param name="mode">The load scene mode used to load the scene</param>
    void ChangeVisualOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeVisualRepresentationTo(currentVisualState);
    }

    public void Start()
    {
        ChangeVisualRepresentationTo(currentVisualState);
    }

    bool qKeyReleased = true;
    public void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && qKeyReleased)
        {
            ToggleVisualRepresentation();
            qKeyReleased = false;
        }
        else if (Keyboard.current.qKey.wasReleasedThisFrame)
        {
            qKeyReleased = true;
        }
    }

    /// <summary>
    /// Changes the visual representation to either 3D or 2D specifically
    /// </summary>
    /// <param name="visualStateToSwitchTo">The visual state to switch to</param>
    public void ChangeVisualRepresentationTo(VisualState visualStateToSwitchTo)
    {
        if (visualStateToSwitchTo == VisualState.TwoD)
        {
            currentVisualState = VisualState.TwoD;
            foreach(ThreeDToTwoDRepresentation representation in FindObjectsOfType<ThreeDToTwoDRepresentation>(true))
            {
                representation.SwitchRepresentationTo2D();
            }
        }
        else if (visualStateToSwitchTo == VisualState.ThreeD)
        {
            currentVisualState = VisualState.ThreeD;
            foreach (ThreeDToTwoDRepresentation representation in FindObjectsOfType<ThreeDToTwoDRepresentation>(true))
            {
                representation.SwitchRepresentationTo3D();
            }
        }
    }

    /// <summary>
    /// Toggles the current visual representation. If in 3D, switches it to 2D and vice versa.
    /// </summary>
    public void ToggleVisualRepresentation()
    {
        if (currentVisualState == VisualState.ThreeD)
        {
            ChangeVisualRepresentationTo(VisualState.TwoD);
        }
        else if (currentVisualState == VisualState.TwoD)
        {
            ChangeVisualRepresentationTo(VisualState.ThreeD);
        }
    }
}
