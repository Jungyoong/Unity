using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotDamage : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "Player" && collider.gameObject.tag != "shot" && collider.gameObject.tag != "weapon" && collider.gameObject.tag != "enemy")
            Destroy(gameObject);
    }
}
