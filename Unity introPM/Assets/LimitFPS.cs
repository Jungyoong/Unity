using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    Screen.SetResolution (1920, 1080, false);
    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = 165;
    }
}
