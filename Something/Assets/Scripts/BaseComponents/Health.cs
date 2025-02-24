using UnityEngine;

public class Health : MonoBehaviour
{
    public CoverLogic coverLogic;

    public float maxHealth;
    public float health;
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            if (coverLogic != null && coverLogic.inCover)
                coverLogic.OutCover();
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!coverLogic.inCover || coverLogic.isPeeking)
            health -= damage;
        else if (coverLogic.cover != null)
        {
            coverLogic.cover.health -= damage * 0.75f;
            health -= damage * 0.25f;
        }
            
    }
}
