using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenSceneReferenceContainer : MonoBehaviour
{
    public static BetweenSceneReferenceContainer instance;

    /* Things not to be destroyed */
    public GameObject playerStats;
    public GameObject enemyStats;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(instance);
    }
}
