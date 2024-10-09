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

    void Awake()
    {
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
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && invincibility == false && collision.gameObject.GetComponent<EnemyAI>() != null && collision.gameObject.GetComponent<EnemyAI>().enemyType == EnemySO.EnemyType.chasing)
        {
            invincibility = true;
            Invoke("InvincibilityUpTime", invincibilityTime);

            currentHealth -= collision.gameObject.GetComponent<EnemyAI>().damage;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "EnemyShot" && invincibility == false)
        {
            invincibility = true;
            Invoke("InvincibilityUpTime", invincibilityTime);

            currentHealth -= collider.GetComponent<EnemyShotDamage>().damage;
            healthBar.SetHealth(currentHealth);
        }
        if((currentHealth < maxHealth) && collider.gameObject.tag == "healthPickup")
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
