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



    public int health;
    public int damage;
    float timeBetweenAttacks;
    bool alreadyAttacked;
    float sightRange, attackRange;
    bool playerInSightRange, playerInAttackRange;
    
    private void Start()
    {
        player = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>().rbInstanceTransform.transform;
        enemyType = enemyStats.enemyType;

        Setup();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void AttackPlayer()
    {

        orientation.LookAt(player);

        if (!alreadyAttacked && enemyType != EnemyType.quadSided)
        {
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(BurstShoot(orientation.transform.forward, 0.2f * i, 1));
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        if (!alreadyAttacked && enemyType == EnemyType.quadSided)
        {   
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(BurstShoot(transform.forward, 0.2f * i, 1));
                StartCoroutine(BurstShoot(transform.forward, 0.2f * i, -1));
                StartCoroutine(BurstShoot(transform.right, 0.2f * i, 1));
                StartCoroutine(BurstShoot(transform.right, 0.2f * i, -1));
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
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

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void Setup()
    {
        sightRange = enemyStats.sightRange;
        attackRange = enemyStats.attackRange;
        timeBetweenAttacks = enemyStats.attackSpeed;
        health = enemyStats.health;
        damage = enemyStats.damage;
    }
}
