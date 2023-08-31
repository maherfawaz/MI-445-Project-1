using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
public class Interactable : MonoBehaviour
{
    [Tooltip("The visual prompt to show when the player is able to interact with this object")]
    public GameObject interactionPrompt;
    [Tooltip("The unity events to make happen when the player interacts with this game object")]
    public UnityEvent unityEventsToInvokeOnInteraction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMovement playerController = other.GetComponent<PlayerMovement>();
            playerController.SetCurrentInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMovement playerController = other.GetComponent<PlayerMovement>();
            playerController.UnSetActiveInteractable(this);
        }
    }

    public void ShowPrompt()
    {
        if (interactionPrompt)
        {
            interactionPrompt.SetActive(true);
        }
    }

    public void HidePrompt()
    {
        if (interactionPrompt)
        {
            interactionPrompt.SetActive(false);
        }
    }

    public void DoInteraction()
    {
        unityEventsToInvokeOnInteraction.Invoke();
    }
}
