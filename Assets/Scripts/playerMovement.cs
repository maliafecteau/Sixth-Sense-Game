using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class playerMovement : MonoBehaviour
{

    CharacterController playerController;
    [SerializeField] InputActionAsset inputActions;
    InputActionMap actionMap;
    InputAction moveAction;
    float currentVelocity;
    private Vector3 velocity;
    private bool groundedPlayer;
    private float gravityForce = -9.81f;

    [SerializeField] float speed = 3;
    [SerializeField] float rotationSmoothTime;

    void Awake()
    {
        actionMap = inputActions.FindActionMap("Player");
        moveAction = actionMap.FindAction("Move");
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

    void FixedUpdate()
    {
        groundedPlayer = playerController.isGrounded;

        if (groundedPlayer && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        velocity.y += gravityForce * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);

        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Only rotate when we actually have movement input, otherwise keep last rotation.
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        }

        playerController.Move(moveDirection * Time.deltaTime * speed);
    }
}