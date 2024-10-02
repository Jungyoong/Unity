using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    InstantiateManager instantiateManager;
    public Slider slider;

    void Start()
    {
       instantiateManager = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>();
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health) //Constructor
    {
        slider.value = health;
    }
    // Start is called before the first frame update

}
