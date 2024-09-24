using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    [Header("Weapon Stats")]
    public GameObject shot;
    public float shotSpeed = 15f;
    public float upwardShotSpeed = 1f;
    public int weaponID = 0;
    public int fireMode = 0;
    public float fireRate = 0;
    public float currentClip = 0;
    public float clipSize = 0;
    public float maxAmmo = 0;
    public float currentAmmo = 0;
    public float reloadAmt = 0;
    public float bulletLifespan = 0;
    public bool canFire = true;

    public string hitName;

    
    public Transform weaponSlot;
    public PlayerController playerControl;
    public Transform attackPoint;
    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && canFire && currentClip > 0 && weaponID >= 0)
        {
            canFire = false;

            GameObject projectile = Instantiate(shot, attackPoint.position, cam.rotation);

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            Vector3 forceDirection = cam.transform.forward;

            playerControl.rb.AddForce(-forceDirection * 20, ForceMode.Impulse);

            RaycastHit hit;

            if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
            {   
                forceDirection = (hit.point - attackPoint.position).normalized;
                hitName = hit.collider.gameObject.name;
                Debug.Log(hitName);
            }

            Vector3 forceToAdd = forceDirection * shotSpeed + forceDirection * upwardShotSpeed;

            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

            currentClip--;

            StartCoroutine("cooldownFire");

            Destroy(projectile, bulletLifespan);
        }

        if (Input.GetKeyDown(KeyCode.R))
            reloadClip();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "weapon")
        {
            collider.gameObject.transform.SetPositionAndRotation(weaponSlot.position, weaponSlot.rotation);
            
            collider.gameObject.transform.SetParent(weaponSlot);

            switch(collider.gameObject.name)
            {
                case "weapon1":
                    weaponID = 0;
                    shotSpeed = 1000f;
                    upwardShotSpeed = 150f;
                    fireMode = 0;
                    fireRate = 0.25f;
                    currentClip = 20;
                    clipSize = 20;
                    maxAmmo = 400;
                    currentAmmo = 200;
                    reloadAmt = 20;
                    bulletLifespan = 1;
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
