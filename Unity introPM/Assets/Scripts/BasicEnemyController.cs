using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{

    public PlayerCollide Hit;
    public int health = 3;
    public int maxHealth = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (Hit.hitName == "Enemy")
        {
            Hit.hitName = null;
            health--;
        }
    }
}