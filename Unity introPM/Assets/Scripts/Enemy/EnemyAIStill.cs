using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemySO;

public class EnemyAIStill : MonoBehaviour
{
    internal Transform player;
    public LayerMask whatIsPlayer;
    public GameObject projectile;
    public EnemySO enemyStats;
    public Transform orientation;
    public EnemyType enemyType;



    public int damage;
    float timeBetweenAttacks;
    bool alreadyAttacked;
    float sightRange, attackRange;
    bool playerInSightRange, playerInAttackRange;
    bool spawnDelay = false;
    
    private void Start()
    {
        player = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>().rbInstanceTransform.transform;
        enemyType = enemyStats.enemyType;

        Setup();

        Invoke("SpawnDelay", 2.5f);
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && playerInAttackRange && spawnDelay && !alreadyAttacked) AttackPlayer();
    }

    private void AttackPlayer()
    {
        switch (enemyType)
        {
            case EnemyType.quadSided:
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(BurstShoot(transform.forward, 0.2f * i, 1));
                    StartCoroutine(BurstShoot(transform.forward, 0.2f * i, -1));
                    StartCoroutine(BurstShoot(transform.right, 0.2f * i, 1));
                    StartCoroutine(BurstShoot(transform.right, 0.2f * i, -1));
                }

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
                
            break;

            case EnemyType.laser:
                orientation.LookAt(player);
                Invoke(nameof(LaserAttack), 2f);

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);

                break;

            default:
                orientation.LookAt(player);

                for (int i = 0; i < 3; i++)
                    StartCoroutine(BurstShoot(orientation.transform.forward, 0.2f * i, 1));
                
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            
            break;
        }


    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void LaserAttack()
    {
        if (Physics.SphereCast(transform.position, 1f, orientation.transform.forward, out RaycastHit hit, 400))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    hit.collider.GetComponent<PlayerHP>().TakeDamage(damage);
                }
            }
    }

    IEnumerator BurstShoot(Vector3 direction, float delayTime, int negative)
    {
        yield return new WaitForSeconds(delayTime);

        GameObject projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
        Rigidbody rb = projectileObject.GetComponent<Rigidbody>();
        projectileObject.GetComponent<EnemyShotDamage>().damage = damage;

        rb.AddForce(direction * 32f * negative, ForceMode.Impulse);
        Destroy(projectileObject, 3f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Debug.DrawRay(transform.position, orientation.transform.forward);
    }

    private void Setup()
    {
        sightRange = enemyStats.sightRange;
        attackRange = enemyStats.attackRange;
        timeBetweenAttacks = enemyStats.attackSpeed;
        damage = enemyStats.damage;
    }

    void SpawnDelay()
    {
        spawnDelay = true;
    }

}
