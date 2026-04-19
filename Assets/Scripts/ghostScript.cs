using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class GhostScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] UIDocument dialogueUI;
    [SerializeField]playerMovement player;
    public GameManager gameManager;
    public int phase = 0;

    VisualElement root;
    Button giveItemBtn;
    Button askSecretBtn;
    Button exitBtn;

    void OnEnable()
    {
        if (dialogueUI == null)
        {
            Debug.LogWarning("GhostScript: dialogueUI is not assigned.");
            return;
        }

        root = dialogueUI.rootVisualElement;
        if (root == null)
        {
            Debug.LogWarning("GhostScript: rootVisualElement is null.");
            return;
        }
        root.style.display = DisplayStyle.None;

        // cache buttons and validate
        giveItemBtn = root.Q<Button>("GiveItemBtn");
        askSecretBtn = root.Q<Button>("SecretBtn");
        exitBtn = root.Q<Button>("ExitBtn");

        Debug.Log($"GhostScript: GiveItemBtn={(giveItemBtn != null)}, SecretBtn={(askSecretBtn != null)}, ExitBtn={(exitBtn != null)}");

        if (giveItemBtn != null)
        {
            giveItemBtn.clicked -= OnGiveItemClicked;
            giveItemBtn.clicked += OnGiveItemClicked;
        }

        if (askSecretBtn != null)
        {
            askSecretBtn.clicked -= OnAskSecretClicked;
            askSecretBtn.clicked += OnAskSecretClicked;
        }

        if (exitBtn != null)
        {
            exitBtn.clicked -= OnExitClicked;
            exitBtn.clicked += OnExitClicked;
        }

        // initial text
        var textBox = root.Q<Label>("textBox");
        if (textBox != null)
        {
            Debug.Log("Setting initial ghost dialogue text.");
            textBox.text = "...H-hello?  Can you help me?  I can't- remember much... but I think I was making something...";
            Debug.Log(textBox.text);
        }
    }

    void OnDisable()
    {
        if (giveItemBtn != null) giveItemBtn.clicked -= OnGiveItemClicked;
        if (askSecretBtn != null) askSecretBtn.clicked -= OnAskSecretClicked;
        if (exitBtn != null) exitBtn.clicked -= OnExitClicked;
    }

    public void Interact()
    {
        if (dialogueUI == null)
        {
            Debug.LogWarning("GhostScript.Interact: dialogueUI is not assigned.");
            return;
        }

        root = dialogueUI.rootVisualElement;
        if (root == null) return;

        root.style.display = DisplayStyle.Flex;

        Debug.Log("You interact with the ghost: UI shown");
    }

    void OnGiveItemClicked()
    {
        Debug.Log("GiveItemBtn clicked");
        if (player == null)
        {
            Debug.LogWarning("GiveItemClicked: player reference is null.");
            return;
        } else
        {
            Debug.Log($"Player holding item: {player.holdingItem}, correct item: {player.correctItem}");
        }

        if (player.holdingItem)
        {
            if (player.correctItem)
            {
                GiveCorrectItem();
            }
            else
            {
                GiveWrongItem();
            }
        }
        else
        {
            Debug.Log("You have no item to give.");
        }
    }

    void OnAskSecretClicked()
    {
        Debug.Log("AskSecretBtn clicked");
        AskSecret();
    }

    void OnExitClicked()
    {
        Debug.Log("ExitBtn clicked");
        if (root == null) root = dialogueUI?.rootVisualElement;
        root.style.display = DisplayStyle.None;
        Debug.Log("You stop interacting with the ghost.");
    }

    public void GiveCorrectItem()
    {
        gameManager?.AddTrust(50);
        gameManager?.ChangeMood(25);
        Debug.Log("Ghost is pleased");
        phase += 1;
    }

    public void GiveWrongItem()
    {
        gameManager?.ChangeMood(-20);
        Debug.Log("Ghost is upset");
    }

    public void AskSecret()
    {
        gameManager?.AskAboutSecret();
        Debug.Log("You ask the ghost about the secret.");
    }
}
