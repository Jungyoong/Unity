using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
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

    
    public int damage;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    bool spawnDelay = false;
    
    void Start()
    {
        player = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>().rbInstanceTransform.transform;
        agent = GetComponent<NavMeshAgent>();
        enemyType = enemyStats.enemyType;

        Invoke(nameof(SpawnDelay), 2.5f);

        Setup();
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (spawnDelay)
        {
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
        
    }

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }

    void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
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

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void Setup()
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

