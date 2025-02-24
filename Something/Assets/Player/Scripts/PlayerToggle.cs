using UnityEngine;

public class PlayerToggle : MonoBehaviour
{
    public ComputerPlayerScript computerPlayerScript;
    public PlayerMovement playerMovement;

    public void DisableComponents()
    {
        if (computerPlayerScript.cover != null && computerPlayerScript.cover.coverQueue != null)
            computerPlayerScript.cover.coverQueue = null;
        computerPlayerScript.enabled = true;
        
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<PlayerOrientation>().enabled = false;
        
        playerMovement.isUsing = false;
        playerMovement.moveDirection = Vector3.zero;
        playerMovement.rb.angularVelocity = Vector3.zero;
    }
    public void EnableComponents()
    {
        playerMovement.moveDirection = Vector3.zero;

        GetComponent<PlayerAttack>().enabled = true;
        GetComponent<PlayerOrientation>().enabled = true;
        
        computerPlayerScript.enabled = false;
        playerMovement.isUsing = true;
    }
}
