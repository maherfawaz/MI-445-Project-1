using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script contains a function that can be called to flip the rotation of a game object in the Y
/// </summary>
public class YRotationFlipper : MonoBehaviour
{
    public List<Transform> targetTransforms;
    
    public void FlipOnY()
    {
        foreach(Transform targetTransform in targetTransforms)
        {
            targetTransform.rotation = Quaternion.Euler(targetTransform.eulerAngles.x, targetTransform.eulerAngles.y + 180, targetTransform.eulerAngles.z);
        }
    }
}
