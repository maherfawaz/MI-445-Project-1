using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the polish of a projectile for when it collides with something
/// </summary>
public class Projectile : MonoBehaviour
{
    [Tooltip("The rotater which rotates the three D projectile")]
    public ContinualRotater threeDRotater;
    [Tooltip("The rotater which rotates the two D projectile")]
    public ContinualRotater twoDRotater;
    [Tooltip("The prefab of the effect to create when destroying a projectile from hitting something")]
    public GameObject projectileDestroyEffect;

    private void Start()
    {
        threeDRotater.SetXRotationSpeedToRandomValue(-180, 180);
        threeDRotater.SetYRotationSpeedToRandomValue(-180, 180);
        threeDRotater.SetZRotationSpeedToRandomValue(-180, 180);

        twoDRotater.SetZRotationSpeedToRandomValue(-180, 180);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (projectileDestroyEffect)
        {
            Instantiate(projectileDestroyEffect, transform.position, Quaternion.identity, null);
        }
        Destroy(gameObject);
    }
}
