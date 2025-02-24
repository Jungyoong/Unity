using Unity.VisualScripting;
using UnityEngine;

public class CoverDetection : MonoBehaviour
{
    public Cover cover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<CoverLogic>() != null)
            collider.GetComponent<CoverLogic>().cover = cover;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<CoverLogic>().cover == cover)
            collider.GetComponent<CoverLogic>().cover = null;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.GetComponent<CoverLogic>() != null && collider.GetComponent<CoverLogic>().cover == null)
            collider.GetComponent<CoverLogic>().cover = cover;
    }
}
