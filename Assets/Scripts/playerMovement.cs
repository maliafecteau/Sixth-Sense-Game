using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class playerMovement : MonoBehaviour
{

    public CharacterController playerController;
    public GhostScript ghostie;
    [SerializeField] InputActionAsset inputActions;
    InputActionMap actionMap;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction interactAction;
    LevelExit exitScript;
    [SerializeField] LevelEvents levelEvent;
    public itemScript heldItem;

    float currentVelocity;
    private float verticalVelocity;
    public bool isGrounded;
    public bool IsJumping { get; private set; }
    private float gravity = -9.8f;

    [SerializeField] Transform cameraTransform;
    [SerializeField] public float speed = 2;
    [SerializeField] float jumpHeight = 0.2f;
    [SerializeField] float rotationSmoothTime;
    

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public Vector3 currentPosition;



    void Awake()
    {
        actionMap = inputActions.FindActionMap("Player");
        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");
        sprintAction = actionMap.FindAction("Sprint");
        interactAction = actionMap.FindAction("Interact");
    }

    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }
    void Start()
    {
        playerController = GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Exit"))
        {

            exitScript = collision.gameObject.GetComponent<LevelExit>();
            if (exitScript != null && levelEvent != null)
            {
                levelEvent.InvokeLevelExit(exitScript);
            }
        }
        
        if (collision.gameObject.CompareTag("interactable"))
        {
            var item = collision.gameObject.GetComponent<itemScript>();
            if (item != null && tooltipScript.Instance != null)
            {

                tooltipScript.Instance.Show(item.itemName);
            }
            item.GetComponent<Rigidbody>().sleepThreshold = 0.0f; //adjust the sleep threshold to prevent the item from going to sleep and not being interactable
        }
        if (collision.gameObject.CompareTag("Ghost"))
        {
            ghostie = collision.gameObject.GetComponent<GhostScript>();
            ghostie.Interact();
        }
    }

    void OnTriggerStay(Collider collision) //called once per frame while the player is within the collider of an interactable object
    {
        if (collision.gameObject.CompareTag("interactable"))
        {
            
            if (interactAction.WasPressedThisFrame())
            {
                var item = collision.gameObject.GetComponent<itemScript>();
                if (heldItem == null) //if the player is not holding an item, set the held item to the item they are interacting with
                {
                    heldItem = item;
                    heldItem.PickUp();
                    tooltipScript.Instance.Hide();
                }
                else
                {
                    if (item != heldItem) //if the player is holding an item and tries to interact with a different item, do nothing
                    {
                        Debug.Log("picked up " + item.itemName);
                        heldItem.PutBack(); //put back the currently held item
                        heldItem = item; //set the held item to the new item
                        item.PickUp();
                        tooltipScript.Instance.Hide();
                        return;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        // Hide tooltip when leaving an interactable
        if (collision.gameObject.CompareTag("interactable"))
        {
            collision.gameObject.GetComponent<Rigidbody>().sleepThreshold = 0.005f; //reset the sleep threshold when leaving the interactable
            if (tooltipScript.Instance != null)
                tooltipScript.Instance.Hide();
            Debug.Log($"Player left interactable object {collision.gameObject.name}");
        }
    }

    void Update()
    {
        isGrounded = playerController.isGrounded;

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2.0f;
        }
        if (jumpAction.IsPressed() && isGrounded)
        {
            IsJumping = true;
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        } else
        {
            IsJumping = false;
        }
            verticalVelocity += gravity * Time.deltaTime;

        Vector3 moveDir = Vector3.zero;
        moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.magnitude > 0.01f)
        {
            if (sprintAction.IsPressed() && isGrounded)
            {
                speed = 5f;
            }
            else
            {
                speed = 2f;
            }
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);
            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        }

        Vector3 velocity = moveDir * speed + Vector3.up * verticalVelocity * gravity * -1;
        playerController.Move(velocity * Time.deltaTime);
    }

    // Call this to remove the currently held item (cleanly) from player and world
    public void RemoveHeldItem()
    {
        if (heldItem == null) return;

        // If your item has its own cleanup, call that; otherwise destroy the GameObject
        heldItem.RemoveFromWorld();

        // Clear player state
        heldItem = null;
    }
}