using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestUpgrade : MonoBehaviour
{
    PlayerUpgradeStats upgrade;


    void Start()
    {
        upgrade = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>().playerUpgradeStats;
    }

    void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.CompareTag("Player"))
        {
            upgrade.semiAutoDamageMulti += 1f;
            upgrade.semiAutoDamageAdd += 10;
            Debug.Log("Upgraded!");
        }
    }
}

