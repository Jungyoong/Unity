using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FirstPersonCam : MonoBehaviour
{
    public CameraPos cameraPos;
    public bool firstPerson;


    // Update is called once per frame
    void Update()
    {
        if (firstPerson)
            transform.SetPositionAndRotation(cameraPos.transform.position, cameraPos.transform.rotation);
    }
}
