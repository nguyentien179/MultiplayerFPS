using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")] 
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement playerMovement;

    [Header("Sliding")] 
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    private float horizontalInput;
    private float verticalInput;

    private bool isSliding = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftControl) && (horizontalInput != 0 || verticalInput != 0) &&
            playerMovement.movementState == PlayerMovement.MovementState.sprinting)
        {
            StartSlide();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && isSliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if(isSliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        isSliding = true;
        
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }
    private void StopSlide()
    {
        isSliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }

    private void SlidingMovement()
    {
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(inputDir.normalized * slideForce, ForceMode.Force);
        
        slideTimer -= Time.deltaTime;
        if(slideTimer <= 0)
            StopSlide();
    }
}
