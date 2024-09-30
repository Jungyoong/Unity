using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneReferences", menuName = "ScriptableObjects/SceneReferences", order = 1)]
public class SceneReferences : ScriptableObject
{

    public Rigidbody rb;
    public PlayerCollide playerCollide;
    public Transform cam;
    public Transform weaponSlot;
    public Transform attackPoint;
}
