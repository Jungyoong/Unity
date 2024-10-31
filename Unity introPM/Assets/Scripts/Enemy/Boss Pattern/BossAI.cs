using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossAI : MonoBehaviour
{
    public BossHpBar bossHP;
    public EnemyHP bossHealth;
    public EnemySO bossStats;

    public LaserAttack laserAttack;
    public TrackingAttack trackingAttack;


    public int damage;
    bool cooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        bossHP.SetMaxHP(bossStats.health);
        bossHealth.health = bossStats.health;
    }

    // Update is called once per frame
    void Update()
    {
        bossHP.SetHP(bossHealth.health);

        if (!cooldown)
            StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(4f);

        laserAttack.startAttack = true;

        yield return new WaitForSeconds(10f);
        trackingAttack.attackStart = true;
        trackingAttack.Look();

        cooldown = true;
    }

}
