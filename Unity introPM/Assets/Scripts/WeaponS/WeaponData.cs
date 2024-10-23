using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int weaponID;
    public int damage;
    public float range;
    public float fireRate;
    public float bulletKnockback;
    public int currentClip;
    public int maxClip;
    public int currentAmmo;
    public int maxAmmo;
    public float shotSpeed;
    public float maxCharge;
    public FireMode fireMode;
    public enum FireMode
    {
        auto,
        semiAuto,
        charge,
        melee
    }
    public ShotType shotType;
    public enum ShotType
    {
        projectile,
        hitscan
    }
    public GameObject projectilePreFab;
}
