using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrackingAttack : MonoBehaviour
{
    public GameObject projectile;
    public BossAI bossStats;
    Transform orientation;
    Transform player;


    bool alreadyAttacked = false;
    public bool attackStart = false;
    int damage;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>().rbInstanceTransform.transform;
        orientation = transform.GetChild(0);

        damage = bossStats.damage;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Look()
    {
        orientation.transform.LookAt(player);
        if (!alreadyAttacked && attackStart)

            StartCoroutine(Attack());

    }

    IEnumerator Attack()
    {
        alreadyAttacked = true;

        GameObject projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
        Rigidbody rb = projectileObject.GetComponent<Rigidbody>();
        projectileObject.GetComponent<EnemyShotDamage>().damage = damage;
        rb.AddForce(orientation.transform.forward * 32f, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        float time =+ 0.2f;
        if (time > 5f)
            attackStart = false;

        alreadyAttacked = false;
        Destroy(projectileObject, 2.5f);
    }
}
