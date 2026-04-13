using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class playerMovement : MonoBehaviour
{

    public CharacterController playerController;
    [SerializeField] InputActionAsset inputActions;
    InputActionMap actionMap;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    LevelExit exitScript;
    [SerializeField] LevelEvents levelEvent;

    float currentVelocity;
    private float verticalVelocity;
    public bool isGrounded;
    private float gravity = -9.8f;

    [SerializeField] Transform cameraTransform;
    [SerializeField] float speed = 3;
    [SerializeField] float jumpHeight = 0.1f;
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

    void OnTriggerEnter(Collider  collision)
    {
        if (collision.gameObject.CompareTag("Exit"))
        {

            exitScript = collision.gameObject.GetComponent<LevelExit>();
            levelEvent.InvokeLevelExit(exitScript);
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
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 moveDir = Vector3.zero;
        moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.magnitude > 0.01f)
        {
            if (sprintAction.IsPressed() && isGrounded)
            {
                speed = 7f;
            }
            else
            {
                speed = 3f;
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


}