using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    public int reloadAmt = 30;


    private void OnTriggerEnter(Collider collider)
    {
        PlayerEquip playerEquip = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>().playerEquip;
        
        if(playerEquip.equip && (playerEquip.weaponScript.currentAmmo < playerEquip.weaponScript.maxAmmo) && collider.gameObject.CompareTag("ammoPickup"))
        {
            playerEquip.weaponScript.currentAmmo += reloadAmt;

            if (playerEquip.weaponScript.currentAmmo > playerEquip.weaponScript.maxAmmo)
                playerEquip.weaponScript.currentAmmo = playerEquip.weaponScript.maxAmmo;
            
            Destroy(collider.gameObject);
        }
    }

}
