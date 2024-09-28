using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public WeaponData currentWeapon;
    public Rigidbody rb;
    public PlayerCollide playerCollide;


    public Transform weaponSlot;
    public Transform attackPoint;
    public Transform cam;

    public string hitName;
    public bool equip = false;
    public bool canFire;
    public int weaponID;
    public string weaponName;
    public float currentClip;
    public float maxClip;
    public float currentAmmo;
    public float maxAmmo;
    public int fireMode;
    public int damage;
    public float range;
    public float fireRate;
    public float bulletKnockback;


    void Start()
    {
        weaponID = currentWeapon.weaponID;
        weaponName = currentWeapon.weaponName;
        currentClip = currentWeapon.currentClip;
        maxClip = currentWeapon.maxClip;
        currentAmmo = currentWeapon.currentAmmo;
        maxAmmo = currentWeapon.maxAmmo;
        fireRate = currentWeapon.fireRate;
        fireMode = currentWeapon.fireMode;
        range = currentWeapon.range;
        damage = currentWeapon.damage;
        bulletKnockback = currentWeapon.bulletKnockback;
    }

    void Update()
    {
        equip = playerCollide.equip;

        if (Input.GetMouseButton(0) && canFire && equip && currentClip > 0)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
            reloadClip();
    }
    
    void Shoot()
    {
        canFire = false;
        Vector3 forceDirection = cam.transform.forward;

        //Bullet knockback
        rb.velocity = new Vector3(0, 0, 0);
        rb.AddForce(-forceDirection * bulletKnockback, ForceMode.VelocityChange);

        //checks if the player's crosshair has hit an object
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {   
            forceDirection = (hit.point - attackPoint.position).normalized;
            hitName = hit.collider.gameObject.name;
            Debug.Log(hitName);
            if (hit.collider.gameObject.tag == "basicEnemy")
            {
                BasicEnemyController enemyHP = hit.transform.GetComponent<BasicEnemyController>();
                enemyHP.health -= damage;
            }
        }
        currentClip--;
        StartCoroutine("cooldownFire");
    }


    public void reloadClip()
    {
        if (currentClip >= maxClip)
            return;

        else
        {
            float reloadCount = maxClip - currentClip;

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
    
    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player" && !equip)
        {
            
            //sets the weapon to the player's arm
            gameObject.transform.SetPositionAndRotation(weaponSlot.position, weaponSlot.rotation);
            gameObject.transform.SetParent(weaponSlot);
            canFire = true;
        }
    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}
