using UnityEngine;

public class ghostScript : MonoBehaviour
{
    public GameManager gameManager;

    public void GiveCorrectItem()
    {
        gameManager.AddTrust(20);
        gameManager.ChangeMood(10);
        Debug.Log("Ghost is pleased");
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
