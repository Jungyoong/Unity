using System.Data.Common;
using UnityEngine;

public class Cover : MonoBehaviour
{
    internal GameObject personInCover;
    internal CoverLogic coverLogic;
    public ComputerPlayerScript coverQueue;
    public Transform personCoverPosition;

    public bool inUse;
    public float health;

    void Update()
    {
        if (health <= 0)
        {
            if (coverLogic != null)
                coverLogic.OutCover();
            Destroy(this.gameObject);
        }

        if (coverQueue != null)
        {
            if (coverQueue.coverFound)
                return;
            else
                coverQueue = null;
        }
    }

    public void NoCover()
    {
        coverLogic = null;
        inUse = false;
    }

    public void YesCover(CoverLogic player)
    {
        coverLogic = player;
        inUse = true;
    }
}
