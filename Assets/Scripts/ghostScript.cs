using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class GhostScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    playerMovement player;
    InputActionMap actionMap;
    InputAction interactAction;
    InputAction askSecretAction;
    public GameManager gameManager;
    public int phase = 0;

    private void Awake()
    {
        actionMap = inputActions.FindActionMap("Player");
        interactAction = actionMap.FindAction("Interact");
        askSecretAction = actionMap.FindAction("AskSecret");
    }
    public void Interact()
    {
        Debug.Log("You interact with the ghost.");
        // Here you would typically show a UI for item selection
        if (interactAction.WasPressedThisFrame())
        {
           if (player.holdingItem)
            {
                if (player.correctItem)
                {
                    GiveCorrectItem();
                } else
                {
                    GiveWrongItem();
                }
            } else
            {
               Debug.Log("You have no item to give.");
            }
        }

           if (askSecretAction.WasPressedThisFrame())
            {
                AskSecret();
        }

    }
    public void GiveCorrectItem()
    {
        gameManager.AddTrust(20);
        gameManager.ChangeMood(10);
        Debug.Log("Ghost is pleased");
        phase += 1;
    }

    public void GiveWrongItem()
    {
        gameManager.ChangeMood(-20);
        Debug.Log("Ghost is upset");
    }

    public void AskSecret()
    {
        gameManager.AskAboutSecret();
    }
}
