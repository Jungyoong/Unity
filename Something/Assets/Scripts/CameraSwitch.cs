using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public FirstPersonCam firstPersonCam;
    public GameObject thirdPerson;
    
    public void FirstPerson()
    {
        firstPersonCam.firstPerson = true;
        thirdPerson.SetActive(false);
    }

    public void ThirdPerson()
    {
        firstPersonCam.firstPerson = false;
        thirdPerson.SetActive(true);
    }
}
