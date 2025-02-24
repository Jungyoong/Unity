using TMPro;
using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    public CharacterSelection characterSelection;

    public float health;
    public float damage;
    public float fireRate;
    public float critChance, critDamage;
    public float clipSize, reloadTime;
    public string playerName;

    public void SetValues()
    {
        if (characterSelection.selected == null)
            return;
        if (characterSelection.selected.transform.GetChild(0).GetComponent<TMP_Text>() != null)
            characterSelection.selected.transform.GetChild(0).GetComponent<TMP_Text>().SetText(playerName);
        characterSelection.selected.playerName = playerName;
        characterSelection.selected.health = health;
        characterSelection.selected.damage = damage;
        characterSelection.selected.fireRate = fireRate;
        characterSelection.selected.critChance = critChance;
        characterSelection.selected.critDamage = critDamage;
        characterSelection.selected.clipSize = clipSize;
        characterSelection.selected.reloadTime = reloadTime;
        characterSelection.ChangeStats(characterSelection.selected);
    }

    public void CopyTo(SelectionScript other)
    {
        other.health = health;
        other.damage = damage;
        other.fireRate = fireRate;
        other.critChance = critChance;
        other.critDamage = critDamage;
        other.clipSize = clipSize;
        other.reloadTime = reloadTime;
        other.playerName = playerName;
    }
}
