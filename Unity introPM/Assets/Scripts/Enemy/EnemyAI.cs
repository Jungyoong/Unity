using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using static EnemySO;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    internal Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;
    public EnemySO enemyStats;
    public EnemyType enemyType;
    public Vector3 walkPoint;

    bool walkPointSet;
    public float walkPointRange;

    public int health;
    public int damage;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    private void Start()
    {
        player = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>().rbInstanceTransform.transform;
        agent = GetComponent<NavMeshAgent>();
        enemyType = enemyStats.enemyType;

        Setup();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {

        agent.SetDestination(transform.position);
        transform.LookAt(player);


        if (!alreadyAttacked && enemyType != EnemyType.chasing)
        {
            GameObject projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody rb = projectileObject.GetComponent<Rigidbody>();
            projectileObject.GetComponent<EnemyShotDamage>().damage = damage;
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            Destroy(projectileObject, 2.5f);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
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
        Gizmos.DrawWireSphere(transform.position , attackRange);
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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "shot")
            TakeDamage(collider.gameObject.GetComponent<PlayerShotDamage>().damage);
    }
}

