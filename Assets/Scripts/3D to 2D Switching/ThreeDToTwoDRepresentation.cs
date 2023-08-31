using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ThreeDToTwoDRepresentation : MonoBehaviour
{
    [Header("Visual Representations")]
    public GameObject threeDRepresentation;
    public GameObject twoDRepresentation;

    private void Start()
    {
        if (ThreeDToTwoDSwitchController.instance != null)
        {
            if (ThreeDToTwoDSwitchController.instance.currentVisualState == ThreeDToTwoDSwitchController.VisualState.ThreeD)
            {
                SwitchRepresentationTo3D();
            }
            else
            {
                SwitchRepresentationTo2D();
            }
        }
        else
        {
            SwitchRepresentationTo3D();
        }
    }

    public void SwitchRepresentationTo3D()
    {
        if (threeDRepresentation != null && twoDRepresentation != null)
        {
            threeDRepresentation.SetActive(true);
            twoDRepresentation.SetActive(false);
        }

    }

    public void SwitchRepresentationTo2D()
    {
        if (threeDRepresentation != null && twoDRepresentation != null)
        {
            threeDRepresentation.SetActive(false);
            twoDRepresentation.SetActive(true);
        }
    }
}
