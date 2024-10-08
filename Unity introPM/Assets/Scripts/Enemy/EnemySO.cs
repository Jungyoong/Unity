using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/EnemyStats")]
public class EnemySO : ScriptableObject
{
    public float attackSpeed;
    public float sightRange;
    public float attackRange;
    public int health;
    public int damage;
}
