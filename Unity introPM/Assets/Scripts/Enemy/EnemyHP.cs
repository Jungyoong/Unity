using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int health;
    internal InstantiateManager instantiateManager;

    // Start is called before the first frame update
    void Start()
    {
        instantiateManager = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>();
        instantiateManager.enemyCount++;
        switch (gameObject)
        {
            case var _ when gameObject.GetComponent<EnemyAI>() != null:
                health = gameObject.GetComponent<EnemyAI>().enemyStats.health;
                break;
            case var _ when gameObject.GetComponent<EnemyAIStill>() != null:
                health = gameObject.GetComponent<EnemyAIStill>().enemyStats.health;
                break;
            case var _ when gameObject.GetComponent<BossAI>() != null:
                health = gameObject.GetComponent<BossAI>().bossStats.health;
                break;
            default:
                Debug.LogWarning("No matching component found!");
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log(damage);

        if (health < 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
        instantiateManager.enemyCount--;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("shot"))
            TakeDamage(collider.gameObject.GetComponent<PlayerShotDamage>().damage);
    }
}
