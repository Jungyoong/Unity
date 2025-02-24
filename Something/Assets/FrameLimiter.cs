using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 165;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
