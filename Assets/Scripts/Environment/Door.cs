using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Door : MonoBehaviour
{
    public bool openDoor;

    public void SetDoorState(bool newDoorState)
    {
        openDoor = newDoorState;
    }

    void Update() {
        if (openDoor) {
            Destroy(gameObject);
        }
    }
}
