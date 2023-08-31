using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ContinualRotater : MonoBehaviour
{
    [Header("Rotation Speed Settings")]
    [Tooltip("The X Rotation Speed to rotate the game object by in degrees per second")]
    public float xRotationSpeed = 0f;
    [Tooltip("The Y Rotation Speed to rotate the game object by in degrees per second")]
    public float yRotationSpeed = 0f;
    [Tooltip("The Z Rotation Speed to rotate the game object by in degrees per second")]
    public float zRotationSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        UpdateRotation();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }

    public void UpdateRotation()
    {
        transform.Rotate(xRotationSpeed * Time.deltaTime, yRotationSpeed * Time.deltaTime, zRotationSpeed * Time.deltaTime);
    }

    public void SetXRotationSpeedToRandomValue(float minimumValue, float maximumValue)
    {
         xRotationSpeed = Random.Range(minimumValue, maximumValue);
    }

    public void SetYRotationSpeedToRandomValue(float minimumValue, float maximumValue)
    {
        yRotationSpeed = Random.Range(minimumValue, maximumValue);
    }

    public void SetZRotationSpeedToRandomValue(float minimumValue, float maximumValue)
    {
        zRotationSpeed = Random.Range(minimumValue, maximumValue);
    }
}
