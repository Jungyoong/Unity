using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float Xsensitivity;
    public float Ysensitivity;

    InstantiateManager instantiateManager;
    internal Transform orientation;

    float xRotation;
    float yRotation;


    private void Start()
    {
        instantiateManager = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>();
        orientation = instantiateManager.orientation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * Xsensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * Ysensitivity;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }


}
