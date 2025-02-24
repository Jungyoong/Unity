using UnityEngine;

public class PlayerCoverLogic : CoverLogic
{
    internal PlayerSwitch player;
    void Start()
    {
        player = GetComponentInParent<PlayerSwitch>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (movement.isUsing)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!inCover && cover != null && !cover.inUse && !inCooldown)
                    InCover();
                else if (inCover && !inCooldown)
                    OutCover();
            } 

            if (inCover)
            {
                if (Input.GetKeyDown(KeyCode.W) && !isPeeking)
                    isPeeking = true;
                if (Input.GetKeyDown(KeyCode.S) && isPeeking)
                    isPeeking = false;   
            }

            if (!isPeeking || !inCover)
                player.cameraSwitch.ThirdPerson();
            else
                player.cameraSwitch.FirstPerson();
        }
        else
        {
            if (!inCover && cover != null && !cover.inUse && !inCooldown && canCover)
                InCover();
        }
    }

    void FixedUpdate()
    {
        if (inCover)
            PeekingCheck();
    }
}
