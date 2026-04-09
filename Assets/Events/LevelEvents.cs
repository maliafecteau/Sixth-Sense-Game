using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LevelEvents : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Camera mainCam;

    // This method is called to trigger the exit
    public void InvokeLevelExit(LevelExit teleportPos)
    {
        if (player == null)
        {
            Debug.LogWarning("LevelEvents.InvokeLevelExit: player is not assigned.");
            return;
        }

        // If player has a CharacterController, temporarily disable it while teleporting
        var controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            player.transform.position = teleportPos.nextLevelPos;
            controller.enabled = true;
        }
        else
        {
            player.transform.position = teleportPos.nextLevelPos;
        }

        Debug.Log("Player teleported to: " + teleportPos.nextLevelPos);

        if (mainCam != null)
        {
            mainCam.transform.position = teleportPos.cameraTransform;
            mainCam.transform.rotation = teleportPos.cameraRotation;
        }
        else
        {
            Debug.LogWarning("LevelEvents.InvokeLevelExit: mainCam is not assigned.");
        }
    }
}
