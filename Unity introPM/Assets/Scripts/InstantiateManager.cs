using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InstantiateManager : MonoBehaviour
{
    public GameObject rbPreFab;
    public GameObject camPreFab;


    internal Rigidbody rb;
    internal PlayerEquip playerEquip;
    internal GameObject camInstance;
    internal Camera mainCam;
    internal CameraControl cameraControl;
    internal Transform weaponSlot;
    internal Transform attackPoint;
    internal Transform cam;
    internal Transform camPos;
    internal Transform orientation;
    internal PlayerHP playerHP;

    // Start is called before the first frame update
    void Start()
    {
        //handles the instantiate values for the player PreFab
        GameObject rbInstance = Instantiate(rbPreFab);
        playerEquip = rbInstance.AddComponent<PlayerEquip>();
        orientation = rbInstance.transform.GetChild(0);
        camPos = rbInstance.transform.GetChild(1);
        rb = rbInstance.GetComponent<Rigidbody>();
        playerHP = rbInstance.GetComponent<PlayerHP>();

        //handles the instantiate values for the camera PreFab
        camInstance = Instantiate(camPreFab);
        mainCam = camInstance.GetComponent<Camera>();
        cameraControl = camInstance.GetComponent<CameraControl>();
        cam = camInstance.GetComponent<Transform>();
        weaponSlot = camInstance.transform.GetChild(0);
        attackPoint = camInstance.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
