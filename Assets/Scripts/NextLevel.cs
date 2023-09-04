using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public Scene scene; // Current scene
    public int sceneCurrent; // Current scene number
    public int sceneNext; // Next scene number

    void Start()
    {
        scene = SceneManager.GetActiveScene(); // Gets active scene
        sceneCurrent = scene.buildIndex; // Sets sceneCurrent to number of the active scene
        sceneNext = scene.buildIndex + 1; // Sets sceneNext to number of the next scene
    }

    // Load next scene when goal is reached
    void OnCollisionEnter(Collision coll)
    {
        print("Colided");
        if (coll.gameObject.CompareTag("Player")) SceneManager.LoadScene(sceneNext);
    }
}
