using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{

    public Health health;
    public Transform fill;


    void Update()
    {
        float healthRatio = health.health / health.maxHealth;
        fill.localScale = new Vector3(healthRatio, 1, 1);
    }
}
