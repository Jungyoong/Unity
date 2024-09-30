using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private InstantiateManager instantiateManager;
    internal Transform cameraPosition;
    
    // Update is called once per frame
    void Start()
    {
        instantiateManager = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>();
        cameraPosition = instantiateManager.camPos;
    }
    void Update()
    {
        instantiateManager.camInstance.transform.position = cameraPosition.position;
    }
}
