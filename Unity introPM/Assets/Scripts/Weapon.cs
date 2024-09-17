using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int weaponID = 0;
    public int fireMode = 0;
    public float fireRate = 0;
    public float currentClip = 0;
    public float clipSize = 0;
    public float maxAmmo = 0;
    public float currentAmmo = 0;
    public float reloadAmt = 0;
    public bool canFire = true;

    
    public Transform weaponSlot;
    public PlayerController playerControl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && canFire && currentClip > 0)
        {
            canFire = false;
            currentClip--;
            StartCoroutine("cooldownFire");
        }

        if (Input.GetKeyDown(KeyCode.R))
            reloadClip();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "weapon")
        {
            collider.gameObject.transform.position = weaponSlot.position;
            
            collider.gameObject.transform.SetParent(weaponSlot);

            switch(collider.gameObject.name)
            {
                case "weapon1":
                    weaponID = 0;
                    fireMode = 0;
                    fireRate = 0.25f;
                    currentClip = 20;
                    clipSize = 20;
                    maxAmmo = 400;
                    currentAmmo = 200;
                    reloadAmt = 20;
                    break;

                default:
                    break;
            }
        }
            
        if((currentAmmo < maxAmmo) && collider.gameObject.tag == "ammoPickup")
        {
            currentAmmo += reloadAmt;

            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;
            
            Destroy(collider.gameObject);
        }
    
    }
    
    public void reloadClip()
    {
        if (currentClip >= clipSize)
            return;

        else
        {
            float reloadCount = clipSize - currentClip;

            if (currentAmmo < reloadCount)
            {
                currentClip += currentAmmo;
                currentAmmo = 0;
                return;
            }

            else
            {
                currentClip += reloadCount;
                currentAmmo -= reloadCount;
                return;
            }
        }
    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}
