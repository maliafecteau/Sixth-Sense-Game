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

        if (playerMove.moveInput.magnitude > 3.0f)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
        if (playerMove.isGrounded == false)
        {
            playerAnimator.SetBool("isJump", true);
        } 
        else
        {
            playerAnimator.SetBool("isJump", false);
        }
    }
}
