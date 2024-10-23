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
    public bool isMoving;

    
    [Header("Ground Check")]
    internal Transform orientation;
    InstantiateManager instantiateManager;
    public LayerMask whatIsGround;
    public float playerHeight;
    bool onGround;

    [Header("Keybinds")]
    public KeyCode sprintKey = KeyCode.LeftShift;

    Vector3 moveDirection;
    internal Rigidbody rb;
    internal PlayerStamina playerStamina;

    public MovementState state; 
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }
    
    void Awake()
    {
        instantiateManager = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        orientation = instantiateManager.orientation;
        rb = instantiateManager.rb;
        playerStamina = instantiateManager.playerStamina;
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics.Raycast(transform.position, -transform.up, playerHeight * 0.5f + 0.2f, whatIsGround);

        SpeedControl();
        StateHandler();
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (onGround)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;

        
    }

    public void StateHandler()
    {
        if (onGround && Input.GetKey(sprintKey) && playerStamina.currentStamina > 0 && isMoving)
        {
            state = MovementState.sprinting;
            playerStamina.isSprint = true;
            moveSpeed = sprintSpeed;
        }
        else if (onGround)
        {
            state = MovementState.walking;
            playerStamina.isSprint = false;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
            playerStamina.isSprint = false;
        }

    }

    public void MovePlayer()
    {
        moveDirection = (orientation.forward * Input.GetAxisRaw("Vertical")) + (orientation.right * Input.GetAxisRaw("Horizontal"));

        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
            isMoving = true;
        else
            isMoving = false;

        if (onGround)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!onGround)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    public void Jump()
    {
        if (onGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
    
    }


    public void SpeedControl()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatvel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatvel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    public void AirSpeedControl()
    {
        Vector3 yFlatvel = new Vector3(0f, rb.velocity.y, 0f);

        if (yFlatvel.magnitude > airSpeed)
        {
            Vector3 yLimitedVel = yFlatvel.normalized * airSpeed;
            rb.velocity = new Vector3(rb.velocity.x, yLimitedVel.y, rb.velocity.z);
        }
    }
}