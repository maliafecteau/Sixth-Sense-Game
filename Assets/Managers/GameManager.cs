using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int trust = 0;
    public int mood = 50; // 0 = angry, 100 = calm

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
