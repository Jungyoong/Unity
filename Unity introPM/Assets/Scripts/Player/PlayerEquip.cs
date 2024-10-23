using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    internal WeaponSystem weaponScript;
    Transform weaponSlot;


    public bool equip;
    public int equipID;
    GameObject equippedWeapon;

    // Start is called before the first frame update
    void Start()
    {
        weaponSlot = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>().weaponSlot;
        equip = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && equip)
        {
            equip = false;
            weaponScript.equip = false;
            equipID = 0;

            // Detach the weapon from the player
            equippedWeapon.transform.SetParent(null);

            // Enable physics on the dropped weapon
            Rigidbody rb = equippedWeapon.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }    
            // Clear references
            equippedWeapon = null;
            weaponScript = null;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("weapon") && !equip)
        {
            // Attach the weapon to the weapon slot
            collider.transform.SetPositionAndRotation(weaponSlot.position, weaponSlot.rotation);
            collider.transform.SetParent(weaponSlot);

            // Get the weapon script and set it up
            weaponScript = collider.gameObject.GetComponent<WeaponSystem>();
            equippedWeapon = collider.gameObject;

            equip = true;
            weaponScript.equip = true;
            weaponScript.canFire = true; 

            equipID = weaponScript.weaponID;

            // Disable physics on the equipped weapon
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }
}
