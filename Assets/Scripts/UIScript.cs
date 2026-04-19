using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class UIScript : MonoBehaviour
{
    UIDocument UIDocument;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIDocument = GetComponent<UIDocument>();

        var startBtn = UIDocument.rootVisualElement.Q<Button>("StartBtn");
        startBtn.clicked += OnStartClicked;
    }

    private void OnStartClicked()
    {
        SceneManager.LoadScene("GameScene");
    }
}
