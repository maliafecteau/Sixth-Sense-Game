using UnityEngine;
using UnityEngine.UIElements;

public class GhostScript : MonoBehaviour
{
    [SerializeField] UIDocument dialogueUI;
    [SerializeField] playerMovement player;
    public GameManager gameManager;
    public int phase = 0;
    itemScript playerItem;

    VisualElement root;
    Button giveItemBtn;
    Button askSecretBtn;
    Button exitBtn;
    Button yesBtn;

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
        yesBtn = root.Q<Button>("YesBtn");

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
        if (yesBtn != null)
        {
            yesBtn.clicked -= GiveCorrectItem;
            yesBtn.clicked += GiveCorrectItem;
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

        var root = dialogueUI.rootVisualElement;
        if (root == null) return;

        root.style.display = DisplayStyle.Flex;

        Debug.Log("You interact with the ghost: UI shown");

        var textBox = root.Q<Label>("textBox");
        if (textBox != null)
        {
            if (phase == 0)
            {
                textBox.text = "...H-hello?  Can you help me?  I can't- remember much... but I think I was making something...?";
                Debug.Log(textBox.text);
            }
            else if (phase == 1)
            {
                textBox.text = "Oh, hi again...  Have you found what I was using to make this?  \nI need to finish it.";
                Debug.Log(textBox.text);
            }
            else if (phase == 2)
            {
                textBox.Q<Label>("textBox").text = "These pens... they're the same ones I was using!  All the colours match, look!\nBut they're mine... what were they doing in your room?\n\nNever mind, thats not important.  We can finish it together, if you like?";
                root.Q<Button>("GiveItemBtn").style.display = DisplayStyle.None;
                root.Q<Button>("YesBtn").style.display = DisplayStyle.Flex; // advance dialogue button to reflect new phase
            }
            else if (phase == 3)
            {
                    
                textBox.text = "The card is finished now, finally... and I can remember some more things.  You look... familiar, but much older than I remember.  Thank you for helping me...";
                Debug.Log(textBox.text);
                root.Q<Button>("GiveItemBtn").style.display = DisplayStyle.None;
                root.Q<Button>("ExitBtn").style.display = DisplayStyle.None;
                root.Q<Button>("YesBtn").style.display = DisplayStyle.None;
            }
        }
    }

    void OnGiveItemClicked()
    {
        Debug.Log("GiveItemBtn clicked");
        if (player == null)
        {
            Debug.LogWarning("GiveItemClicked: player reference is null.");
            return;
        }

        // FIXED: comparison, not assignment
        if (player.heldItem == null)
        {
            Debug.Log("You have no item to give.");
            return;
        }

        playerItem = player.heldItem;
        Debug.Log($"Player is holding item: {playerItem.itemName}, correct item 1: {playerItem.isCorrectItem1}, correct item 2: {playerItem.isCorrectItem2}");

        if (phase == 0)
        {
            if (playerItem.isCorrectItem1)
            {
                dialogueUI.rootVisualElement.Q<Label>("textBox").text = "Oh... that's right!  I was drawing something for some reason... I think I was making a gift for- ...nevermind.  \nIt's not finished though... what was I using to make this, again?";
                GiveCorrectItem();
                player.RemoveHeldItem();
            }
            else if (playerItem.isCorrectItem2)
            {
                dialogueUI.rootVisualElement.Q<Label>("textBox").text = "That seems right, I think I was using these pens to make something... But I dont know what it was.\nI needed <i>that</i> instead.";
                player.RemoveHeldItem();
                gameManager?.ChangeMood(-20);
            }
            else
            {
                GiveWrongItem();
                player.RemoveHeldItem();
            }
        }
        else if (phase == 1)
        {
            if (playerItem.isCorrectItem2)
            { 
                GiveCorrectItem();
                player.RemoveHeldItem();
                Interact(); // advance dialogue immediately to reflect new phase
            }
            else
            {
                GiveWrongItem();
                player.RemoveHeldItem();
            }
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
        gameManager?.AddTrust(33);
        gameManager?.ChangeMood(25);
        Debug.Log("Ghost is pleased");
        phase += 1;
        if (phase == 3)
        {
            Interact(); // advance dialogue immediately to reflect new phase
        }
    }

    public void GiveWrongItem()
    {
        dialogueUI.rootVisualElement.Q<Label>("textBox").text = "That's not what I needed... You don't understand- <i>no-one</i> does!";
        gameManager?.ChangeMood(-20);
        Debug.Log("Ghost is upset");
    }

    public void AskSecret()
    {
        gameManager?.AskAboutSecret();
        Debug.Log("You ask the ghost about the secret.");
    }
}
