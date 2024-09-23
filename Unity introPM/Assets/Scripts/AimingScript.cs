using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingScript : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public TrackedReference attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int ammo;
    public float throwCooldown;

    [Header("Shooting")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public float shootForce;
    public float shootUpwardForce;

    bool readyToShoot;


    private void Start()
    {
        readyToShoot = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(shootKey) && readyToShoot && ammo > 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        //Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            //forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection * shootForce + transform.up * shootUpwardForce;

        //projectileRB.AddForce(forceToAdd, ForceMode.Impulse);
    }
}
