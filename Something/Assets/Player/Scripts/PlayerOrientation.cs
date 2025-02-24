using UnityEngine;

public class PlayerOrientation : MonoBehaviour
{
    public Transform attackPointOrientation;
    public Transform cameraOrientation;

    void Start()
    {
        cameraOrientation = GameObject.Find("PlayerCamera").transform;
    }

    void Update()
    {
        attackPointOrientation.rotation = cameraOrientation.rotation;
    }
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, cameraOrientation.eulerAngles.y, 0);
    }
}
