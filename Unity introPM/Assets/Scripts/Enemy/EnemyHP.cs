using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int health;


    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<EnemyAI>() != null)
            health = gameObject.GetComponent<EnemyAI>().enemyStats.health;
        else
            health = gameObject.GetComponent<EnemyAIStill>().enemyStats.health;
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
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("shot"))
            TakeDamage(collider.gameObject.GetComponent<PlayerShotDamage>().damage);
    }
}
