using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] LayerMask groundLayers;

    public float walkspeed = 0f;
    public float sprintspeed = 10f; // Sprint speed
    public float jumpHeight = 3f;
    private float gravity = -50f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount = 0;
    private int maxJumps = 2; // Allow double jump

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            jumpCount = 0; // Reset jump count when grounded
        }

        float x = Input.GetAxis("Horizontal");

        // Right is the red Axis, forward is the blue axis
        Vector3 move = transform.right * x;

        // Check if the sprint key (left shift) is held down
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintspeed : walkspeed;

        characterController.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && (isGrounded || jumpCount < maxJumps))
        {
            // The equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
        }

        // Add gravity
        velocity.y += gravity * Time.deltaTime;

        // Vertical velocity
        characterController.Move(velocity * Time.deltaTime);
    }
}