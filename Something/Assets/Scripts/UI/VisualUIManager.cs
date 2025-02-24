using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VisualUIManager : MonoBehaviour
{

    [Header("Reload and ammo stuff")]
    public Stats stats;
    public SliderScript slider;
    public GameObject sliderFill;
    public TMP_Text ammoText;

    [Header("Enemy stuff")]
    public TMP_Text enemyAmount;



    void Update()
    {
        ammoText.SetText(stats.computerPlayerScript.clipAmount + " / " + stats.computerPlayerScript.clipSize);
        SetReloadSlider();
    }

    public void SetReloadSlider()
    {
        slider.SetMaxValue2(stats.computerPlayerScript.reloadTime);
        slider.SetValue(stats.computerPlayerScript.reloadTimer);

        sliderFill.SetActive(stats.computerPlayerScript.reloadTimer > 0);
    }
}
