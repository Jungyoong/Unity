
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    public Transform cameraPos;

    public float Xsensitivity;
    public float Ysensitivity;
    public float xRotation;
    public float yRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPos.position;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * Xsensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * Ysensitivity;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        if (yRotation > 360f || yRotation < -360f)
            yRotation = 0;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
