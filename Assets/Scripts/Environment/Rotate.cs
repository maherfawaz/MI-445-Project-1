using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Rotate : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public bool doRotation;
    public Vector3 rotate;

    public void SetBridgeState(bool newBridgeState)
    {
        doRotation = newBridgeState;
    }

    // Update is called once per frame
    void Update()
    {
        if (doRotation)
        {
            //float degrees = 0;
            rotate = new Vector3(x, y, z);

            transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rotate, Time.deltaTime);
        }
    }
}
