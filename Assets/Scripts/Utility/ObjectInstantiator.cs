using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles instantiating a gameobject whenever the associated function is called and is meant to be used
/// when something like a visual effect needs to be created through a unity event
/// </summary>
public class ObjectInstantiator : MonoBehaviour
{
    /// <summary>
    /// Instatiates a prefab at this script's transform's position with 0,0,0 rotation and no parent
    /// </summary>
    /// <param name="prefabToCreate">The prefab which is instantiated</param>
    public void InstantiateGameObject(GameObject prefabToCreate)
    {
        Instantiate(prefabToCreate, transform.position, Quaternion.identity, null);
    }
}
