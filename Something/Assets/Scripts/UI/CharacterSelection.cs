using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] players;
    public GameObject playerSelect, warningScreen, playerStats;
    public VerticalLayoutGroup vert;

    public SelectionScript selected;
    public Transform playerList;
    public Transform playerSave;

    public TMP_Text health, damage, fireRate, critChance, critDamage, maxClip, reloadSpeed;

    public void Start()
    {
        Transform playerSaves = GameObject.Find("PlayerSave").transform;

        if (playerSaves != null)
        {
            for (int i = 0; i < playerSaves.childCount; i++)
            {
                AddPlayer();
                playerSaves.GetChild(i).GetComponent<SelectionScript>().CopyTo(players[i].GetComponent<SelectionScript>());
                players[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText(players[i].GetComponent<SelectionScript>().playerName);
            } 
        }

    }

    public void AddPlayer()
    {
        if (players.Length > 3)
        {
            warningScreen.SetActive(true);
            return;
        }

        List<GameObject> _players = new(players);
        
        GameObject newPlayer = Instantiate(playerSelect);
        newPlayer.transform.GetChild(0).GetComponent<TMP_Text>().SetText("Player");
        newPlayer.transform.SetParent(playerList);

        EventTrigger.Entry entry = new()
        {
            eventID = EventTriggerType.PointerClick
        };

        entry.callback.AddListener((eventData) => CharacterSelect(newPlayer.GetComponent<SelectionScript>()));
        newPlayer.GetComponent<EventTrigger>().triggers.Add(entry); 

        _players.Add(newPlayer);
        players = _players.ToArray();

        LayoutRebuilder.ForceRebuildLayoutImmediate(vert.GetComponent<RectTransform>());
    }

    public void RemovePlayer()
    {
        if (selected == null)
            return;
        
        List<GameObject> _players = new(players);

        if (_players.Contains(selected.gameObject))
        {
            _players.Remove(selected.gameObject);
            Destroy(selected.gameObject);
            selected = null;
        }

        players = _players.ToArray();

        LayoutRebuilder.ForceRebuildLayoutImmediate(vert.GetComponent<RectTransform>());
    }

    public void CharacterSelect(SelectionScript selectedPlayer)
    {   
        if (selected != null)
            selected.GetComponent<RawImage>().color = Color.white;
        selected = selectedPlayer;
        selected.GetComponent<RawImage>().color = Color.gray;
        ChangeStats(selected);
    }

    public void ChangeStats(SelectionScript selectedPlayer)
    {
        health.SetText("Health: " + selectedPlayer.health);
        damage.SetText("Damage: " + selectedPlayer.damage);
        fireRate.SetText("Fire rate: " + selectedPlayer.fireRate);
        critChance.SetText("Critical Chance: " + selectedPlayer.critChance);
        critDamage.SetText("Critical Damage: " + selectedPlayer.critDamage);
        maxClip.SetText("Max Clip: " + selectedPlayer.clipSize);
        reloadSpeed.SetText("Reload Time: " + selectedPlayer.reloadTime);
    }

    public void CloseWarning()
    {
        warningScreen.SetActive(false);
    }

    public void SavePlayers()
    {
        for (int i = 0; i < playerSave.childCount; i++)
            Destroy(playerSave.GetChild(i).gameObject);
        for (int i = 0; i < players.Length; i++)
        {
            GameObject _player = Instantiate(playerStats);
            players[i].GetComponent<SelectionScript>().CopyTo(_player.GetComponent<SelectionScript>());
            _player.transform.SetParent(playerSave);
        }
        DontDestroyOnLoad(playerSave);
    }
}
