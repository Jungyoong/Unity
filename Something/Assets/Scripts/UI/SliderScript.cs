using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }
    
    public void SetMaxValue2(float value)
    {
        slider.maxValue = value;
    }
}
