using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : SliderScript
{
    public TMP_Text text;

    public bool dead;
    
    void Update()
    {
        if (slider.value <= 0)
            dead = true;
        else
            dead = false;
    }
}
