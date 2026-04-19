using UnityEngine;

public class itemScript : MonoBehaviour
{
    [SerializeField] public string itemName = "Item";
    [Tooltip("Used by NPC logic when there are two possible correct items")]
    public bool isCorrectItem1;
    public bool isCorrectItem2;

    // State
    public bool PickedUp { get; private set; }

    // Called by the player when they pick up the item
    public void PickUp()
    {
        PickedUp = true;
        // Make the item "held" visually - disable physics and hide
        // You can change this to parent to player instead of disabling.
        gameObject.SetActive(false);
    }

    // Called if the player puts the item back into the world
    public void PutBack()
    {
        PickedUp = false;
        gameObject.SetActive(true);
    }

    // Remove the item from the world permanently (or you can implement pooling)
    public void RemoveFromWorld()
    {
        Destroy(gameObject);
    }
}

