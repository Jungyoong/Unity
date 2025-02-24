using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
    internal Node<GameObject> head;
    internal Node<GameObject> current;
    public GameObject playerObject;
    public TMP_Text damageText;
    public CameraPos view;
    public CameraSwitch cameraSwitch;
    public GameManager gameManager;
    public PlayerList playerList;
    public Stats stats;
    public Transform cameraPos;
    public Transform starterSpawn;
    public Transform playerSave;

    public bool limit;
    public int limitAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSave = GameObject.Find("PlayerSave").transform;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SwitchViewForward();

        if (Input.GetKeyDown(KeyCode.Q))
            SwitchViewBackward();

    }

    public void SwitchViewForward()
    {
        current.Data.GetComponent<PlayerToggle>().DisableComponents();
        current = current.Next;
        current.Data.GetComponent<PlayerToggle>().EnableComponents();
        SetValue();

        SwitchView();
        SetCamera();
        playerList.SwitchPlayerForward();
    }
    public void SwitchViewBackward()
    {   
        current.Data.GetComponent<PlayerToggle>().DisableComponents();
        current = current.Previous;
        current.Data.GetComponent<PlayerToggle>().EnableComponents();
        SetValue();

        SwitchView();
        SetCamera();
        playerList.SwitchPlayerBackward();
    }

    void SetCamera()
    {
        view.cameraPos = current.Data.transform.GetChild(0);
        view.yRotation = current.Data.transform.eulerAngles.y;
        view.xRotation = current.Data.transform.eulerAngles.x;
        cameraPos.rotation = current.Data.transform.rotation;
    }

    public void SwitchView()
    {
        if (current.Data.GetComponent<CoverLogic>().isPeeking)
            cameraSwitch.FirstPerson();
        else
            cameraSwitch.ThirdPerson();
    }

    public void RemoveNode(GameObject player)
    {
        if (head.Next == head)
        {
            gameManager.GameOver();
            return;
        }
        if (head.Data == player)
            head = head.Next;
        if (current.Data == player)
            SwitchViewForward();
        head.RemoveNode(player, head);
        Destroy(player);
    }

    public void SetValue()
    {
        stats.computerPlayerScript = current.Data.GetComponent<ComputerPlayerScript>();
        stats.playerHealth = current.Data.GetComponent<PlayerHealth>();
    }

    public void SetStats(SelectionScript stats, ComputerPlayerScript _playerStats, Health health)
    {
        _playerStats.damage = stats.damage;
        _playerStats.fireRate = stats.fireRate;
        _playerStats.critChance = stats.critChance;
        _playerStats.critDamage = stats.critDamage;
        _playerStats.clipSize = stats.clipSize;
        _playerStats.reloadTime = stats.reloadTime;
        health.maxHealth = stats.health;

        _playerStats.clipAmount = stats.clipSize;
        _playerStats.playerName = stats.playerName;
    }
    
    public void Initialize()
    {
        int length = limit && limitAmount < playerSave.childCount ? limitAmount : playerSave.childCount;
        List<PlayerHealth> playerHealthList = new();

        head = new(Instantiate(playerObject), null, null);
        head.Data.transform.SetParent(transform);
        head.Data.transform.position = starterSpawn.position;
        StartCoroutine(Delay(head.Data.GetComponent<Rigidbody>()));
        head.Next = head;
        head.Previous = head;
        current = head;

        view.cameraPos = current.Data.transform.GetChild(0);
        SetStats(playerSave.GetChild(0).GetComponent<SelectionScript>(), current.Data.GetComponent<ComputerPlayerScript>(), current.Data.GetComponent<Health>());
        current.Data.GetComponent<PlayerToggle>().EnableComponents();
        playerHealthList.Add(current.Data.GetComponent<PlayerHealth>());
        current.Data.GetComponent<PlayerAttack>().damageText = damageText;

        for (int i = 1; i < length; i++)
        {
            head.AddNode(Instantiate(playerObject), head);
            current = current.Next;
            current.Data.transform.SetParent(transform);
            current.Data.transform.position = new(starterSpawn.position.x + (i * 4f), starterSpawn.position.y, starterSpawn.position.z);
            current.Data.GetComponent<PlayerToggle>().DisableComponents();
            SetStats(playerSave.GetChild(i).GetComponent<SelectionScript>(), current.Data.GetComponent<ComputerPlayerScript>(), current.Data.GetComponent<Health>());
            StartCoroutine(Delay(head.Data.GetComponent<Rigidbody>()));
            playerHealthList.Add(current.Data.GetComponent<PlayerHealth>());
            current.Data.GetComponent<PlayerAttack>().damageText = damageText;
        }
        current = current.Next;

        playerList.playerHealths = playerHealthList.ToArray();
        playerList.InitializeList();
        playerList.Initialize();
        SetValue();
    }

    IEnumerator Delay(Rigidbody rb)
    {
        yield return null;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }
}
