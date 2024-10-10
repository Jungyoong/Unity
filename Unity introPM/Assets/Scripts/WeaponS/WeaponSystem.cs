using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using static WeaponData;

public class WeaponSystem : MonoBehaviour
{
    public WeaponData currentWeapon;

    InstantiateManager instantiateManager;

    public LayerMask ignoreRaycast;

    internal Rigidbody rb;
    internal Transform attackPoint;
    internal Transform weaponSlot;
    internal Transform cam;
    internal GameObject projectilePreFab;

    

    public string hitName;
    public bool equip = false;
    public bool canFire;
    public int weaponID;
    public string weaponName;
    public float currentClip;
    public float maxClip;
    public float currentAmmo;
    public float maxAmmo;
    public int damage;
    public float range;
    public float fireRate;
    public float bulletKnockback;
    public float shotSpeed;


    public FireMode fireMode;
    public ShotType shotType;



    void Awake()
    {
        instantiateManager = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>();
        ignoreRaycast = LayerMask.GetMask("Player", "Ignore Raycast");
    }
    void Start()
    {
        Setup();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && canFire && equip && fireMode == FireMode.auto && currentClip > 0)
            Shoot();

        if (Input.GetMouseButtonDown(0) && canFire && equip && fireMode == FireMode.semiAuto && currentClip > 0)
            Shoot();
        
        if (Input.GetKeyDown(KeyCode.R))
            reloadClip();
    }
    
    void Shoot()
    {
        canFire = false;
        Vector3 forceDirection = cam.transform.forward;

        //Bullet knockback
        BulletKnockback(forceDirection);

        //checks if the player's crosshair has hit an object
        RaycastHit hit;

        if (shotType == ShotType.hitscan && Physics.Raycast(cam.position, cam.forward, out hit, 500f, ~ignoreRaycast))
        {   
            if (hit.collider.gameObject.tag == "Enemy")
                DealDamage(hit);
        }

        if (shotType == ShotType.projectile)
        {
            Vector3 forceToAdd = forceDirection * shotSpeed + cam.transform.up * 1;
            GameObject projectile = Instantiate(projectilePreFab, attackPoint.position, cam.rotation);
            projectile.gameObject.GetComponent<PlayerShotDamage>().damage = damage;
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
            Destroy(projectile, 2.5f);
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
    
    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    void BulletKnockback(Vector3 forceDirection)
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.AddForce(-forceDirection * bulletKnockback, ForceMode.VelocityChange);
    }

    void DealDamage(RaycastHit hit)
    {
        if (hit.transform.GetComponent<EnemyAI>() != null)
        {
            EnemyAI enemyHP = hit.transform.GetComponent<EnemyAI>();
            enemyHP.TakeDamage(damage);
        }
        if (hit.transform.GetComponent<EnemyAIStill>() != null)
        {
            EnemyAIStill enemyHP = hit.transform.GetComponent<EnemyAIStill>();
            enemyHP.TakeDamage(damage);
        }
    }
    
    public void Setup()
    {
        weaponID = currentWeapon.weaponID;
        weaponName = currentWeapon.weaponName;
        currentClip = currentWeapon.currentClip;
        maxClip = currentWeapon.maxClip;
        currentAmmo = currentWeapon.currentAmmo;
        maxAmmo = currentWeapon.maxAmmo;
        fireRate = currentWeapon.fireRate;
        range = currentWeapon.range;
        damage = currentWeapon.damage;
        shotSpeed = currentWeapon.shotSpeed;
        bulletKnockback = currentWeapon.bulletKnockback;
        rb = instantiateManager.rb;
        attackPoint = instantiateManager.attackPoint;
        weaponSlot = instantiateManager.weaponSlot;
        cam = instantiateManager.cam;
        projectilePreFab = currentWeapon.projectilePreFab;
        fireMode = currentWeapon.fireMode;
        shotType = currentWeapon.shotType;
    }
}
