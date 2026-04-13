using UnityEngine;

public class animationScriptPlayer : MonoBehaviour
{
    Animator playerAnimator;
    playerMovement playerMove;

    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerMove = GetComponent<playerMovement>();
    }

    void Update()
    {
        if (playerMove.moveInput.magnitude > 0.01f)
        {
            playerAnimator.SetBool("isWalking", true);
        }
        else
        {
            playerAnimator.SetBool("isWalking", false);
        }

        if (playerMove.speed > 2.0f)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
        if (playerMove.IsJumping)
        {
            playerAnimator.SetBool("isJump", true);
        } 
        else
        {
            playerAnimator.SetBool("isJump", false);
        }
    }
}
