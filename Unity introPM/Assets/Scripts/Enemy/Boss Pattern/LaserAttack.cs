using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    public GameObject laserWall;
    public Transform endPoint;
    public bool startAttack;
    bool cooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (startAttack && !cooldown)
            StartCoroutine(LaserWallAttack());

    }

    IEnumerator LaserWallAttack()
    {
        laserWall.SetActive(true);
        Vector3 origin = laserWall.transform.position;
        cooldown = true;
        yield return new WaitForSeconds(2f);

        while (Vector3.Distance(laserWall.transform.position, endPoint.position) > 0)
        {
            float step =+ .005f;
            laserWall.transform.position = Vector3.Lerp(laserWall.transform.position, endPoint.position, step);

            if (Vector3.Distance(laserWall.transform.position, endPoint.position) < 1 && laserWall)
            {
                yield return new WaitForSeconds(4f);
                cooldown = false;
                startAttack = false;
                laserWall.transform.position = origin;
                laserWall.SetActive(false);
            }
                
            yield return null; // Wait for the next frame
        }
    
    }
}
