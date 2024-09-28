using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    WeaponSystem weaponScript;
    public int reloadAmt = 30;
    public bool equip;
    public int equipID;
    public float damage;


    // Start is called before the first frame update
    void Start()
    {
        equip = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "weapon" && !equip)
        {
            weaponScript = collider.gameObject.GetComponent<WeaponSystem>();
            UpdateValues();
        }

        if(equip && (weaponScript.currentAmmo < weaponScript.maxAmmo) && collider.gameObject.tag == "ammoPickup")
        {
            weaponScript.currentAmmo += reloadAmt;

            if (weaponScript.currentAmmo > weaponScript.maxAmmo)
                weaponScript.currentAmmo = weaponScript.maxAmmo;
            
            Destroy(collider.gameObject);
        }
    }

    void UpdateValues()
    {
        equipID = weaponScript.weaponID;
        equip = true;
        damage = weaponScript.damage;
    }
}
