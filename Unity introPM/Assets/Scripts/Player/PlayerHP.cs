using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int healthRestore = 20;
    public int maxHealth = 100;
    public int currentHealth;
    public bool invincibility = false;
    public float invincibilityTime = 2f;

    GameObject healthBarObject;
    HealthBar healthBar;
    GameObject gameManagerInstance;
    GameManager gameManager;

    void Awake()
    {
        gameManagerInstance = GameObject.Find("GameManager");
        healthBarObject = GameObject.Find("Health Bar");
        healthBar = healthBarObject.GetComponent<HealthBar>();  //Improve this part later
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth < 1)
        {
            gameManagerInstance.GetComponent<GameManager>().gameOver = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!invincibility)
        {
            invincibility = true;
            currentHealth -= damage;
            Invoke(nameof(InvincibilityUpTime), invincibilityTime);
            healthBar.SetHealth(currentHealth);
        }
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyAI>() != null && collision.gameObject.GetComponent<EnemyAI>().enemyType == EnemySO.EnemyType.chasing)
        {
            TakeDamage(collision.gameObject.GetComponent<EnemyAI>().damage);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("EnemyShot"))
        {
            TakeDamage(collider.gameObject.GetComponent<EnemyShotDamage>().damage);
        }
        if ((currentHealth < maxHealth) && collider.gameObject.CompareTag("healthPickup"))
        {
            currentHealth += healthRestore;

            healthBar.SetHealth(currentHealth);

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            
            Destroy(collider.gameObject);
        }
    }

    private void InvincibilityUpTime()
    {
        invincibility = false;
    }
}
