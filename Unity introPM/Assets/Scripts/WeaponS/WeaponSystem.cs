using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System;
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

    [SerializeField]
    TrailRenderer bulletTrail;

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
    float maxCharge;
    float charge = 0f;
    bool isCharging = false;
    float multiplier;
    float addDamage;

    PlayerUpgradeStats playerUpgradeStats;
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
        if (canFire && equip && currentClip > 0)
        {
            switch (fireMode)
            {
                case FireMode.auto:
                    if (Input.GetMouseButton(0))
                        Shoot(1f);
                    break;
                case FireMode.semiAuto:
                    if (Input.GetMouseButtonDown(0))
                        Shoot(1f);
                    break;
                case FireMode.charge:
                    ChargeShoot();
                    break;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R) && equip)
            reloadClip();
    }
    
    void Shoot(float chargePerc)
    {
        canFire = false;
        Vector3 forceDirection = cam.transform.forward;

        //Bullet knockback
        rb.velocity = Vector3.zero;
        rb.AddForce(-forceDirection * bulletKnockback * chargePerc, ForceMode.Impulse);

        //hitscan weapon logic
        if (shotType == ShotType.hitscan)
        {
            Ray ray = new Ray(weaponSlot.position, cam.forward);
            TrailRenderer trail = Instantiate(bulletTrail, weaponSlot.position, Quaternion.identity);
            Vector3 endPosition;

            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 500f, ~ignoreRaycast))
            {
                endPosition = hit.point;

                if (hit.collider.gameObject.CompareTag("Enemy"))
                    DealDamage(hit, chargePerc);
            }
            else
                endPosition = ray.GetPoint(500f);

            //spawns the bullet trail
            StartCoroutine(SpawnTrail(trail, endPosition));
        }

        //projectile weapon logic
        if (shotType == ShotType.projectile)
        {
            Vector3 forceToAdd = forceDirection * shotSpeed + cam.transform.up * 1;
            GameObject projectile = Instantiate(projectilePreFab, attackPoint.position, cam.rotation);
            projectile.gameObject.GetComponent<PlayerShotDamage>().damage = (int)Math.Ceiling(damage * chargePerc * playerUpgradeStats.projectileDamageMulti);
;
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.AddForce(forceToAdd * chargePerc, ForceMode.Impulse);
            Destroy(projectile, 2.5f);
        }

        currentClip--;
        StartCoroutine(CooldownFire());
    }

    void ChargeShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            rb.useGravity = true;

            isCharging = false;
            if (charge > 0)
            {
                charge /= maxCharge;
                Shoot(charge);
            }
            charge = 0f;
        }

        if (isCharging)
        {
            charge += Time.deltaTime;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.AddForce(-cam.transform.forward * 1f, ForceMode.VelocityChange);
        }
        
        if (charge > maxCharge)
            charge = maxCharge;
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
    
    IEnumerator CooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

   IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 hit)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(startPosition, hit);
        float travelTime = distance / 2000f;
        float time = 0;

        while (time < travelTime)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, hit, time / travelTime);
            time += Time.deltaTime;

            yield return null;
        }
        Trail.transform.position = hit;

        Destroy(Trail.gameObject, Trail.time);
    }


    void DealDamage(RaycastHit hit, float chargePerc)   //improve this later
    {
        EnemyHP enemyHP = hit.transform.GetComponent<EnemyHP>();

        switch (fireMode)
            {
                case FireMode.semiAuto:
                    addDamage = playerUpgradeStats.semiAutoDamageAdd;
                    multiplier = playerUpgradeStats.semiAutoDamageMulti;
                    break;
                case FireMode.auto:
                    addDamage = playerUpgradeStats.autoDamageAdd;
                    multiplier = playerUpgradeStats.autoDamageMulti;
                    break;
            }

        enemyHP.TakeDamage((int)Math.Ceiling((damage + addDamage) * chargePerc * multiplier));
    }
    
    public void Setup()
    {
        weaponID = currentWeapon.weaponID;
        weaponName = currentWeapon.weaponName;
        fireMode = currentWeapon.fireMode;
        shotType = currentWeapon.shotType;

        currentClip = currentWeapon.currentClip;
        maxClip = currentWeapon.maxClip;
        currentAmmo = currentWeapon.currentAmmo;
        maxAmmo = currentWeapon.maxAmmo;
        fireRate = currentWeapon.fireRate;
        range = currentWeapon.range;
        damage = currentWeapon.damage;
        shotSpeed = currentWeapon.shotSpeed;
        bulletKnockback = currentWeapon.bulletKnockback;
        maxCharge = currentWeapon.maxCharge;

        attackPoint = instantiateManager.attackPoint;
        weaponSlot = instantiateManager.weaponSlot;
        projectilePreFab = currentWeapon.projectilePreFab;
        cam = instantiateManager.cam;
        rb = instantiateManager.rb;
        playerUpgradeStats = instantiateManager.playerUpgradeStats;
    }
}
