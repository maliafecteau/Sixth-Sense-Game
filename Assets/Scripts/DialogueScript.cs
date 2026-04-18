using UnityEngine;
using UnityEngine.UIElements;

public class DialogueScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // 1. Get the root visual element from the UIDocument component
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // 2. Query the button by the name you set in UI Builder
        Button whButton = root.Q<Button>("giveItemBtn");

    }

}
