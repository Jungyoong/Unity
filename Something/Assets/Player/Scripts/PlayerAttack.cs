using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;


public enum GunType
{
    Auto, SemiAuto
}
public class PlayerAttack : MonoBehaviour
{
    internal TMP_Text damageText;
    public Transform attackPoint;
    public ComputerPlayerScript stats;
    public PlayerCoverLogic playerCoverLogic;
    public GunType gunType;

    public float damageDone;
    public float timer;

    private bool canAttack = true;

    void Update()
    {
        if ((playerCoverLogic.isPeeking || !playerCoverLogic.inCover) && Input.GetMouseButton(0) && canAttack && !stats.isReloading)
        {
            if (stats.clipAmount > 0)
            {
                canAttack = false;
                StartCoroutine(Attack());
            }
            else
            {
                stats.isReloading = true;
                StartCoroutine(stats.Reload());
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !stats.isReloading && stats.clipAmount < stats.clipSize)
        {
            stats.isReloading = true;
            StartCoroutine(stats.Reload());
        }

        if (damageDone > 0)
        {
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                damageDone = 0;
                damageText.SetText("");
            } 
        }
    }

    IEnumerator Attack()
    {
        stats.clipAmount--;
        Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Ray ray = new(attackPoint.position + (!playerCoverLogic.isPeeking ? attackPoint.TransformDirection(0.8f, 0f, 0) : Vector3.zero), cameraRay.direction);
        
        bool onEnemy = Physics.Raycast(ray, out RaycastHit hit, stats.enemyDetectionRange * 2, stats.enemyLayer);
        if (onEnemy)
        {
            Health health = hit.collider.GetComponent<Health>();
            Hit(health, hit);
        }
        yield return new WaitForSeconds(stats.fireRate);
        canAttack = true;
    }

    void Hit(Health health, RaycastHit hit)
    {
        bool crit = hit.point.y > health.transform.position.y + 0.35f && (health.coverLogic.isPeeking || !health.coverLogic.inCover);
        float multi = (crit ? 1f + stats.critDamage / 100 : 1f) * stats.damage;

        health.TakeDamage(multi);
        damageDone += multi;
        timer = 0;

        damageText.color = crit ? Color.yellow : Color.white;
        damageText.SetText(damageDone.ToString());
    }
}
