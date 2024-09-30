using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InstantiateManager : MonoBehaviour
{
    public GameObject rbPreFab;
    public GameObject camPreFab;


    internal Rigidbody rb;
    internal PlayerCollide playerCollide;
    internal GameObject camInstance;
    internal Transform weaponSlot;
    internal Transform attackPoint;
    internal Transform cam;
    internal Transform camPos;
    internal Transform orientation;

    // Start is called before the first frame update
    void Start()
    {
        //handles the instantiate values for the player PreFab
        GameObject rbInstance = Instantiate(rbPreFab);
        playerCollide = rbInstance.AddComponent<PlayerCollide>();
        orientation = rbInstance.transform.GetChild(0);
        camPos = rbInstance.transform.GetChild(1);
        rb = rbInstance.GetComponent<Rigidbody>();

        //handles the instantiate values for the camera PreFab
        camInstance = Instantiate(camPreFab);
        cam = camInstance.GetComponent<Transform>();
        weaponSlot = camInstance.transform.GetChild(0);
        attackPoint = camInstance.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
