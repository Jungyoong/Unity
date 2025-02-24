using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAndCoverGroup : MonoBehaviour
{
    GameManager gameManager;
    public Transform enemyGroupTransform;
    public Transform coverGroupTransform;
    public GameObject[] enemyObjects;
    public EnemyAndCoverGroup otherGroup;

    bool finished;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        List<GameObject> enemyGroup = new();

        for (int i = 0; i < enemyGroupTransform.childCount; i++)
        {
            enemyGroup.Add(enemyGroupTransform.GetChild(i).gameObject);
        }

        enemyObjects = enemyGroup.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (AllEnemiesNull() && !finished)
        {
            finished = true;
            SwitchCover();
        }

        if (!AllEnemiesNull())
        {
            int amount = 0;

            foreach (GameObject enemy in enemyObjects)
            {   
                if (enemy != null)
                    amount++;
            }
            gameManager.enemyAmount.SetText("Enemies Left: " + amount);
        }
        else
        {
            gameManager.enemyAmount.SetText("Enemies Left: " + 0);
            gameObject.SetActive(false);
        }    
    }

    bool AllEnemiesNull()
    {
        foreach (GameObject enemy in enemyObjects)
        {
            if (enemy != null)
            {
                return false;
            }
        }
        return true;
    }

    public void SwitchCover()
    {
        if (otherGroup != null)
            Invoke(nameof(ToggleOtherGroup), 6f);
        else
            gameManager.Victory();

        if (coverGroupTransform == null)
            return;

        for (int i = 0; i < coverGroupTransform.childCount; i++)
        {
            coverGroupTransform.GetChild(i).rotation = Quaternion.Euler(0, gameManager.direction, 0);
        }
    }

    public void ToggleOtherGroup()
    {
        otherGroup.enemyGroupTransform.gameObject.SetActive(true);
    }
}
