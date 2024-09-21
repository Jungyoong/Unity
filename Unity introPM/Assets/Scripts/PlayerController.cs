using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpHeight;
    public float airMultiplier;
    
    [Header("Ground Check")]
    public Transform orientation;
    public LayerMask whatIsGround;
    public float playerHeight;
    bool onGround;
    
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        movePlayer();
    }


    // Update is called once per frame
    void Update()
    {
        onGround = Physics.Raycast(transform.position, -transform.up, playerHeight * 0.5f + 0.2f, whatIsGround);

        movementInput();
        speedControl();
        if (Input.GetKeyDown(KeyCode.Space))
            jump();

        if (onGround)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;

        
    }

    private void movementInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    public void movePlayer()
    {
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        
        if (onGround)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!onGround)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    public void jump()
    {
        if (onGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
    
    }


    public void speedControl()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatvel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatvel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}