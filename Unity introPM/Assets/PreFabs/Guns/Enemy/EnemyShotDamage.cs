using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotDamage : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "EnemyShot")
            Destroy(gameObject);
    }
}
