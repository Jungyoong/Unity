using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour
{
    public PlayerHealth[] playerHealths;
    public HealthSlider[] healthSliders;
    public GameObject healthSlider;
    public PlayerSwitch playerSwitch;
    public VerticalLayoutGroup verticalLayoutGroup;

    public int counter;


    void Update()
    {
        for (int i = 0; i < playerHealths.Length; i++)
        {
            healthSliders[i].SetValue(playerHealths[i].health);
        }
    }

    public void InitializeList()
    {
        playerSwitch = GameObject.Find("Players").GetComponent<PlayerSwitch>();
        playerSwitch.current = playerSwitch.head;
        
        List<HealthSlider> _healthSliders = new();

        for (int i = 0; i < playerHealths.Length; i++)
        {
            GameObject _healthSlider = Instantiate(healthSlider);
            _healthSlider.transform.SetParent(this.transform);
            _healthSliders.Add(_healthSlider.GetComponent<HealthSlider>());
            _healthSlider.GetComponent<HealthSlider>().text.SetText(playerSwitch.current.Data.GetComponent<ComputerPlayerScript>().playerName);
            playerSwitch.current = playerSwitch.current.Next;
        }
        healthSliders = _healthSliders.ToArray();
    }

    public void Initialize()
    {
        for (int i = 0; i < playerHealths.Length; i++)
        {
            healthSliders[i].SetMaxValue(playerHealths[i].maxHealth);
            healthSliders[i].transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
        healthSliders[0].transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void SwitchPlayerForward()
    {
        if (!healthSliders[counter].dead)
            healthSliders[counter].transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        counter = counter >= healthSliders.Length - 1 ? 0 : counter + 1;

        if (!healthSliders[counter].dead)
        {
            healthSliders[counter].transform.localScale = new Vector3(1f, 1f, 1f);
            LayoutRebuilder.ForceRebuildLayoutImmediate(verticalLayoutGroup.GetComponent<RectTransform>());
        }
        else
            SwitchPlayerForward();
    }

    public void SwitchPlayerBackward()
    {
        if (!healthSliders[counter].dead)
            healthSliders[counter].transform.localScale = new Vector3(0.4f, 0.4f, 1f);

        counter = counter <= 0 ? healthSliders.Length - 1 : counter - 1;

        if (!healthSliders[counter].dead)
        {
            healthSliders[counter].transform.localScale = new Vector3(1f, 1f, 1f);
            LayoutRebuilder.ForceRebuildLayoutImmediate(verticalLayoutGroup.GetComponent<RectTransform>());
        }
        else
            SwitchPlayerBackward();
    }
}
