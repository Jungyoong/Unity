using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float jumpHeight;
    public float airMultiplier;
    public float airSpeed;

    
    [Header("Ground Check")]
    internal Transform orientation;
    private InstantiateManager instantiateManager;
    public LayerMask whatIsGround;
    public float playerHeight;
    bool onGround;

    [Header("Keybinds")]
    public KeyCode sprintKey = KeyCode.LeftShift;

    Vector3 moveDirection;
    internal Rigidbody rb;

    public MovementState state; 
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }
    

    // Start is called before the first frame update
    void Start()
    {
        instantiateManager = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>();
        orientation = instantiateManager.orientation;
        rb = instantiateManager.rb;
    }

    void FixedUpdate()
    {
    }


    // Update is called once per frame
    void Update()
    {
        onGround = Physics.Raycast(transform.position, -transform.up, playerHeight * 0.5f + 0.2f, whatIsGround);

        speedControl();
        stateHandler();
        movePlayer();

        if (Input.GetKeyDown(KeyCode.Space))
            jump();

        if (onGround)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;

        
    }

    public void stateHandler()
    {
        if (onGround && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (onGround)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }

    }

    public void movePlayer()
    {
        moveDirection = (orientation.forward * Input.GetAxisRaw("Vertical")) + (orientation.right * Input.GetAxisRaw("Horizontal"));

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
    public void airSpeedControl()
    {
        Vector3 yFlatvel = new Vector3(0f, rb.velocity.y, 0f);

        if (yFlatvel.magnitude > airSpeed)
        {
            Vector3 yLimitedVel = yFlatvel.normalized * airSpeed;
            rb.velocity = new Vector3(rb.velocity.x, yLimitedVel.y, rb.velocity.z);
        }
    }
}