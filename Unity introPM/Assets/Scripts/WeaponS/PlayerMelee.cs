using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{

    InstantiateManager instantiateManager;
    Transform cam;
    public LayerMask enemy;

    int meleeDamage = 50;
    bool meleeCooldown = false;
    float meleeCooldownTime = 1.2f;

    void Awake()
    {
        instantiateManager = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = instantiateManager.cam;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !instantiateManager.playerEquip.equip && !meleeCooldown) //attack animation here
        {
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 2f))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyHP enemyHP = hit.transform.GetComponent<EnemyHP>();
                    enemyHP.TakeDamage(meleeDamage);
                    meleeCooldown = true;
                }
            }
        }

        if (meleeCooldown)
            Invoke(nameof(MeleeCooldown), meleeCooldownTime);
    }

    void MeleeCooldown()
    {
        meleeCooldown = false;
    }
}
