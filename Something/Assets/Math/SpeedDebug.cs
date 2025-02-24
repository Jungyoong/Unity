using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedDebug : MonoBehaviour
{
    public TMP_Text tMPro;
    public PlayerMovement vel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tMPro = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vel != null)
            tMPro.text = transform.InverseTransformDirection(vel.rb.linearVelocity).ToString();
    }
}
