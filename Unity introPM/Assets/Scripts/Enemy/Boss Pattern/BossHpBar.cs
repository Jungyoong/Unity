using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
        // Start is called before the first frame update
    public Slider slider;

    public void SetMaxHP(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHP(int health)
    {
        slider.value = health;
    }
}
