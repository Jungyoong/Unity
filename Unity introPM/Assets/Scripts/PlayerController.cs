using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRB;
    Camera PlayerCam;

    Vector2 camRotation;


    public bool sprintMode = false;
    private bool isGrounded;

    [Header("Weapon Stats")]
    public bool canFire = true;

    public int doubleJump = 0;
    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sprintMultiplier = 2.5f;
    public float jumpHeight = 2.0f;
    public float groundDetectDistance = 1.5f;


    [Header("User Settings")]
    public bool sprintToggleOption = false;
    public float mouseSensitivity = 2.0f;
    public float Xsensitivity = 2.0f;
    public float Ysensitivity = 2.0f;
    public float camRotationLimit = 90f;
    // Start is called before the first frame update
    void Start()
    {
       myRB = GetComponent<Rigidbody>();
       PlayerCam = transform.GetChild(0).GetComponent<Camera>();

       camRotation = Vector2.zero;
       Cursor.visible = false;
       Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundDetectDistance);
        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

        PlayerCam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

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

    
        if (isGrounded)
            doubleJump = 0;

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || doubleJump < 1)) 
        {
            temp.y = jumpHeight;
            doubleJump += 1;
        }

        myRB.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);


    }

    IEnumerator cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFire = true;
    }

}
