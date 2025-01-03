using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float footstepTimer;
    private float footstepInterval = 0.5f;
    [Header("Movement")] 
    public float moveSpeed;

    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    [Header("Jump")] 
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouch")] 
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    
    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    
    [Header("Ground Check")] 
    public float playerHeight;
    public LayerMask groundLayer;
    bool isGrounded;

    public Transform orientation;
    
    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;
    
    Rigidbody rb;
    
    public MovementState movementState;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
        PlayerInput();
        HandleFootsteps();
        SpeedControl();
        StateHandler();
        
        if(isGrounded)
            rb.drag = groundDrag;
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    
    private void HandleFootsteps()
    {
        // Check if the player is moving
        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Increment the timer while the player is moving
            footstepTimer += Time.deltaTime;

            // If the timer exceeds the interval, play the footstep sound
            if (footstepTimer >= footstepInterval)
            {
                SoundManager.Instance.PlayFootstep(); // Call your SoundManager method
                footstepTimer = 0f; // Reset the timer after playing the sound
            }
        }
        else
        {
            // Reset the timer if the player stops moving
            footstepTimer = 0f;
        }
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
        if (isGrounded && Input.GetKeyDown(sprintKey))
        {
            isSprinting = !isSprinting;
        }
    }
    private bool isSprinting = false;
    private void StateHandler()
    {
        if (isGrounded && isSprinting)
        {
            movementState = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (Input.GetKey(crouchKey))
        {
            movementState = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        
        else if (isGrounded)
        {
            movementState = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else
        {
            movementState = MovementState.air;
        }

    }
    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if(isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        
    }

    private void SpeedControl()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatvel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatvel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f , rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
    
   
}
