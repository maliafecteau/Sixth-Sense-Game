using UnityEngine;
using UnityEngine.UIElements;

public class tooltipScript : MonoBehaviour
{
    public static tooltipScript Instance { get; private set; }

    UIDocument uiDocument;
    Label tooltipLabel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogWarning("tooltipScript: no UIDocument found on the GameObject. Attach this script to the UIDocument GameObject.");
            return;
        }

        tooltipLabel = uiDocument.rootVisualElement.Q<Label>("tooltipLabel");
        if (tooltipLabel == null)
        {
            Debug.LogWarning("tooltipScript: label 'tooltipLabel' not found in the UI Document.");
            return;
        }

        Hide();
    }

    public void Show(string itemName)
    {
        if (tooltipLabel == null)
            return;

        tooltipLabel.text = $"Press E to pick up {itemName}";
        tooltipLabel.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        if (tooltipLabel == null)
            return;

        tooltipLabel.style.display = DisplayStyle.None;
        tooltipLabel.text = "";
    }
}