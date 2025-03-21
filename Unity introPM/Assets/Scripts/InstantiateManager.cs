using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantiateManager : MonoBehaviour
{
    public GameObject rbPreFab;
    public GameObject camPreFab;
    public GameObject uMPreFab;


    internal Transform rbInstanceTransform;
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
    internal PlayerStamina playerStamina;
    internal PlayerUpgradeStats playerUpgradeStats;

    public int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        //handles the instantiate values for the player PreFab
        GameObject rbInstance = Instantiate(rbPreFab);
        rbInstanceTransform = rbInstance.transform;
        orientation = rbInstance.transform.GetChild(0);
        camPos = rbInstance.transform.GetChild(1);
        rb = rbInstance.GetComponent<Rigidbody>();
        playerHP = rbInstance.GetComponent<PlayerHP>();
        playerEquip = rbInstance.GetComponent<PlayerEquip>();
        playerStamina = rbInstance.GetComponent<PlayerStamina>();

        //handles the instantiate values for the camera PreFab
        camInstance = Instantiate(camPreFab);
        mainCam = camInstance.GetComponent<Camera>();
        cameraControl = camInstance.GetComponent<CameraControl>();
        cam = camInstance.GetComponent<Transform>();
        weaponSlot = camInstance.transform.GetChild(0);
        attackPoint = camInstance.transform.GetChild(1);

        GameObject upgradeManagerInstance = Instantiate(uMPreFab);
        playerUpgradeStats = upgradeManagerInstance.GetComponent<PlayerUpgradeStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCount < 1)
            SceneManager.LoadScene(2);
    }
}
