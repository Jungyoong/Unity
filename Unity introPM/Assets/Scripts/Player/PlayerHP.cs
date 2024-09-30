using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int healthRestore = 20;
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
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
