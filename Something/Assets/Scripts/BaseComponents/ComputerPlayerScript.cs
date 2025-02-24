using System.Collections;
using UnityEngine;


public class ComputerPlayerScript : MonoBehaviour
{
    public Cover cover;
    public CoverLogic coverLogic;
    public Movement movement;
    public GameManager gameManager;
    public Health enemy;

    public LayerMask coverLayer, enemyLayer;

    [Header("Detection")]
    public float coverDetectionRange;
    public float enemyDetectionRange;
    public bool enemyFound;
    public bool coverFound;

    [Header("Stats")]
    public float burstFireAmount, burstRate;
    public float damage;
    public float fireRate;
    public float critChance, critDamage;
    public float clipAmount, clipSize, reloadTime;
    internal bool canAttack = true;
    private bool jumpCooldown;
    public bool isReloading;
    public string playerName;
    internal float reloadTimer;

    [Header("Enemy or Player")]
    public bool isEnemy;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        clipAmount = clipSize;
    }
    void Update()
    {
        if (clipAmount < burstFireAmount && !isReloading)
        {
            isReloading = true;
            StartCoroutine(Reload());
        }

        EnemyDetection();

        if (enemyFound)
        {
            HandleEnemyFound();
        }
        else if (movement.inCover)
        {
            coverLogic.canCover = false;
            coverLogic.OutCover();
        }
        else
        {
            if (isEnemy)
            {
                if (gameManager.canMoveForward)
                    transform.rotation = Quaternion.Euler(0, gameManager.direction + 180f, 0);
                else
                {
                    transform.LookAt(gameManager.center);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y ,transform.rotation.eulerAngles.z);
                }  
            }
            else
            {
                if (gameManager.canMoveForward)
                    transform.rotation = Quaternion.Euler(0, gameManager.direction, 0);
                else
                {
                    movement.moveDirection = Vector3.zero;
                    return;
                }
            }
            movement.moveDirection = transform.forward;
        }
        CheckForJump();
    }

    public void ComputerCoverDetection()
    {
        if (cover != null)
            cover.coverQueue = null;

        coverFound = false;
        cover = null;
        float minDistance = 50;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coverDetectionRange, coverLayer);

        foreach (Collider collider in hitColliders)
        {
            Cover foundCover = collider.GetComponent<Cover>();
            Vector3 directionToPlayer = transform.position - foundCover.personCoverPosition.position;
            float coverDistance = Vector3.Distance(transform.position, collider.transform.position);

            if (coverDistance < minDistance && !foundCover.inUse && (foundCover.coverQueue == null || foundCover.coverQueue == this) && Vector3.Dot(collider.transform.forward, directionToPlayer) < 0.5f)
            {
                coverFound = true;
                minDistance = coverDistance;
                cover = foundCover;
            }
        }
        
        if (cover != null)
            cover.coverQueue = this;

        if (coverFound)
        {
            transform.LookAt(cover.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y ,transform.rotation.eulerAngles.z);
        }  
    }

    public void EnemyDetection()
    {
        float minDistance = 100;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyDetectionRange, enemyLayer);

        foreach (Collider collider in hitColliders)
        {
            Health foundEnemy = collider.GetComponent<Health>();
            float enemyDistance = Vector3.Distance(transform.position, collider.transform.position);

            if (enemyDistance < minDistance)
            {
                enemyFound = true;
                minDistance = enemyDistance;
                enemy = foundEnemy;
            }
        }
        enemyFound = enemy != null;
    }

    void HandleEnemyFound()
    {
        transform.LookAt(enemy.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y ,transform.rotation.eulerAngles.z);

        if(!movement.inCover)
        {
            coverLogic.canCover = true;
            ComputerCoverDetection();
            movement.moveDirection = transform.forward * (coverFound ? 1 : 0);
        }

        if (canAttack && !isReloading)
        {
            canAttack = false;
            StartCoroutine(nameof(Attack));
        }
    }

    void CheckForJump()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.forward, 1f, coverLayer) && !jumpCooldown)
        {
            jumpCooldown = true;
            movement.Jump();
            Invoke(nameof(JumpCooldown), 0.5f);
        }  
    }

    public IEnumerator Reload()
    {
        while (reloadTimer < reloadTime)
        {
            reloadTimer += Time.deltaTime;
            yield return null;
        }
        reloadTimer = 0;
        clipAmount = clipSize;
        isReloading = false;
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(burstRate);
        if (movement.isUsing)
        {
            canAttack = true;
            yield break;
        }

        if (coverFound)
            coverLogic.isPeeking = true;

        for (int i = 0; i < burstFireAmount; i++)
        {
            yield return new WaitForSeconds(fireRate);
            clipAmount--;
            bool critHit = Random.Range(1, 100) >= critChance;
            float multi = (critHit ? 1f + critDamage / 100 : 1f) * damage;

            enemy.TakeDamage(multi);
        }

        yield return new WaitForSeconds(burstRate);
        if (movement.isUsing)
        {
            canAttack = true;
            yield break;
        }

        coverLogic.isPeeking = false;
        canAttack = true;
    }

    void JumpCooldown()
    {
        jumpCooldown = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRange);
    }
}
