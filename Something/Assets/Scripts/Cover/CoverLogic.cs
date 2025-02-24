using UnityEngine;

public class CoverLogic : MonoBehaviour
{
    public Cover cover;
    public Movement movement;
    public Transform thisTransform;

    public bool isPeeking;
    public bool firstPerson;
    internal bool inCooldown;
    public float cooldown;
    public bool inCover;
    public bool canCover;

    void Update()
    {
        if (!inCover && cover != null && !cover.inUse && !inCooldown && canCover)
            InCover();
    }
    void FixedUpdate()
    {
        if (inCover)
            PeekingCheck();
    }

    public void PeekingCheck()
    {
        if (isPeeking)
        {
            thisTransform.position = new Vector3(cover.personCoverPosition.position.x, cover.personCoverPosition.position.y + 0.5f, cover.personCoverPosition.position.z);
            thisTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (!isPeeking)
        {
            thisTransform.position = cover.personCoverPosition.position;
            thisTransform.localScale = new Vector3(1f, 0.5f, 1f);
        }
    }

    public void InCover()
    {
        inCooldown = true;

        cover.YesCover(this);
        movement.inCover = true;

        movement.rb.linearVelocity = Vector3.zero;
        movement.rb.constraints = RigidbodyConstraints.FreezeAll;

        thisTransform.position = cover.personCoverPosition.position;

        inCover = true;
        Invoke(nameof(CoverCooldown), cooldown);
    }

    public void OutCover()
    {
        inCooldown = true;

        cover.NoCover();
        movement.inCover = false;
        cover.coverQueue = null;

        movement.rb.constraints = RigidbodyConstraints.FreezeRotation;
        thisTransform.localScale = new Vector3(1f, 1f, 1f);

        inCover = false;
        isPeeking = false;
        Invoke(nameof(CoverCooldown), cooldown);
    }

    public void CoverCooldown()
    {
        inCooldown = false;
    }
}
