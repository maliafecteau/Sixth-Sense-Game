using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public int trust = 0;
    public int mood = 0; // 0 = angry, 100 = calm
    [SerializeField] UIDocument UIoverlay;
    [SerializeField] Texture2D happyTexture;
    [SerializeField] Texture2D happyishTexture;
    [SerializeField] Texture2D neutralTexture;
    [SerializeField] Texture2D angryishTexture;
    [SerializeField] Texture2D angryTexture;

    private VisualElement trustBarMask;
    private VisualElement moodImg;

    void Start()
    {

        trustBarMask = UIoverlay.rootVisualElement.Q<VisualElement>("TrustBarMask");
        moodImg = UIoverlay.rootVisualElement.Q<VisualElement>("moodVisual");
        AddTrust(0);
        ChangeMood(50);
    }
    public void AddTrust(int amount)
    {
        trust += amount;
        trust = Mathf.Clamp(trust, 0, 100);
        trustBarMask.style.width = Length.Percent(trust);
    }

    public void ChangeMood(int amount)
    {
        mood += amount;
        mood = Mathf.Clamp(mood, 0, 100);
        Debug.Log($"Mood changed to {mood}");
        if (mood >= 80)
        {
            moodImg.style.backgroundImage = happyTexture;
        }
        else if (mood >= 60)
        {
            moodImg.style.backgroundImage = happyishTexture;
        }
        else if (mood >= 40)
        {
            moodImg.style.backgroundImage = neutralTexture;
        }
        else if (mood >= 20)
        {
            moodImg.style.backgroundImage = angryishTexture;
        }
        else if (mood >= 0)
        {
            moodImg.style.backgroundImage = angryTexture;
        }

        if (mood <= 0)
        {
            GameOver("angry");
            Debug.LogWarning("Ghost is angry - Game Over");
        }
    }

    public void AskAboutSecret()
    {
        if (trust >= 70)
        {
            Debug.Log("Good Ending");
            GameOver("good");
        }
        else if (trust >= 40)
        {
            Debug.Log("Neutral Ending");
            GameOver("neutral");
        }
        else
        {
            Debug.Log("Bad Ending");
            GameOver("bad");
        }
    }

    void GameOver(string result)
    {
        if (result == "good")
        {

        }
        else if (result == "neutral")
        {

        }
        else if (result == "bad")
        {

        }
        else if (result == "angry")
        {
            Debug.LogWarning("Ghost became hostile - Game Over");
        }
    }

    private void Update()
    {

        // Update the UI with the current mood visualizer
        /*if (UIoverlay != null)
        {
            var moodLabel = UIoverlay.rootVisualElement.Q<Label>("MoodLabel");
            if (moodLabel != null)
            {
                moodLabel.text = $"Mood({mood})";
            }
        }*/
    }
}
