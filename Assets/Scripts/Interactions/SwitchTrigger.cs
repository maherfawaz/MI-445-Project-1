using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
public class SwitchTrigger : MonoBehaviour
{
    public UnityEvent unityEventsOnActivation;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            unityEventsOnActivation.Invoke();
        }
    }
}
