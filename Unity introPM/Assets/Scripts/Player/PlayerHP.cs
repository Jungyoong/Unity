using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int healthRestore = 20;
    public int maxHealth = 100;
    public int currentHealth;

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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "EnemyShot")
        {
            currentHealth -= collider.GetComponent<EnemyShotDamage>().damage;
            healthBar.SetHealth(currentHealth);

            Destroy(collider.gameObject);
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
}
