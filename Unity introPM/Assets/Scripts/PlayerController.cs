using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public DashBar dashBar;
    private Rigidbody myRB;


    public bool sprintMode = false;
    private bool isGrounded;


    public int doubleJump = 0;
    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sprintMultiplier = 2.5f;
    public float jumpHeight = 2.0f;
    public float groundDetectDistance = 1.5f;
    public int maxDash = 1;
    public int currentDash;
    public int dashSpeed = 30000;
    public bool sprintToggleOption = false;

    // Start is called before the first frame update
    void Start()
    {
       myRB = GetComponent<Rigidbody>();

       currentDash = maxDash;
       dashBar.SetMaxDash(maxDash);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundDetectDistance);

        Vector3 temp = myRB.velocity;

        if(!sprintToggleOption)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                sprintMode = true;

            if (Input.GetKeyUp(KeyCode.LeftShift))
                sprintMode = false;
        }

        if(sprintToggleOption)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") > 0)
                sprintMode = true;

            if (Input.GetAxisRaw("Vertical") <= 0)
                sprintMode = false;
        }



        if (!sprintMode)
            temp.x = Input.GetAxisRaw("Vertical") * speed;       

        if (sprintMode)
            temp.x = Input.GetAxisRaw("Vertical") * speed * sprintMultiplier;

        temp.z = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentDash > 0)
            {
                currentDash -= 1;
                myRB.AddForce(transform.forward * dashSpeed, ForceMode.Force);
                dashBar.SetDash(currentDash);
            }
        }
            
        if (isGrounded)
        {
            doubleJump = 0;
        }    

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || doubleJump < 1)) 
        {
            temp.y = jumpHeight;
            doubleJump += 1;
        }

        myRB.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);

    }


}
