using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public int trust = 0;
    public int mood = 50; // 0 = angry, 100 = calm
    public string moodVisualiser = "Neutral";
    [SerializeField] UIDocument UIoverlay;

    void Awake()
    {
        var root = UIoverlay.rootVisualElement;
        root.Q<ProgressBar>("trustBar").value = trust;
        root.Q<VisualElement>("moodVisual").style.backgroundColor = Color.Lerp(Color.red, Color.green, mood / 100f);
        Debug.Log("mood colour is:" + root.Q<VisualElement>("moodVisual").style.backgroundColor);
    }
    public void AddTrust(int amount)
    {
        trust += amount;
        trust = Mathf.Clamp(trust, 0, 100);
    }

    public void ChangeMood(int amount)
    {
        mood += amount;
        mood = Mathf.Clamp(mood, 0, 100);

        if (mood <= 0)
        {
            GameOver();
            Debug.LogWarning("Ghost is angry - Game Over");
        }
    }

    public void AskAboutSecret()
    {
        if (trust >= 70)
        {
            Debug.Log("Good Ending");
        }
        else if (trust >= 40)
        {
            Debug.Log("Neutral Ending");
        }
        else
        {
            Debug.Log("Bad Ending");
        }
    }

    void GameOver()
    {
        Debug.Log("Ghost became hostile - Game Over");
    }
}
