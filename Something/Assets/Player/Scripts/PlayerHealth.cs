using UnityEngine;

public class PlayerHealth : Health
{
    internal PlayerSwitch playerSwitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSwitch = GetComponentInParent<PlayerSwitch>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            if (coverLogic.inCover)
                coverLogic.OutCover();
            playerSwitch.RemoveNode(this.gameObject);
        }
    }
}
