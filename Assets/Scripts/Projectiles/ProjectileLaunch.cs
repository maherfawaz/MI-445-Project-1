using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script controls launching a projectile when input is received
/// </summary>
public class ProjectileLaunch : MonoBehaviour
{
    [Header("Where to Instatiate")]
    [Tooltip("What transform to launch the projectile from")]
    public Transform launchFrom;

    [Header("Projectile Settings")]
    [Tooltip("The projectile prefab to create")]
    public GameObject projectilePrefab;
    [Tooltip("The speed with which the projectile is launched")]
    public float launchSpeed = 20f;
    [Header("Input Actions")]
    public InputAction launch;

    private bool triggered = false;

    private void OnEnable()
    {
        launch.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (launch.triggered)
            {
                triggered = true;
                // actually launch the projectile 
                // apply force of strength launchSpeed in direction the player is facing
                GameObject launchedProjectile = Instantiate(projectilePrefab, launchFrom.position, Quaternion.identity);
                Rigidbody launchedProjectileRigidbody = launchedProjectile.GetComponent<Rigidbody>();
                launchedProjectileRigidbody.velocity = launchFrom.forward * launchSpeed;
            }
            else
            {
                triggered = false;
            }
        }
    }
}
