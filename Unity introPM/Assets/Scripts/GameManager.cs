using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;

    InstantiateManager instantiateManager;
    public GameObject pauseMenu;

    public Image healthBar;
    public TextMeshProUGUI clipCounter;
    public TextMeshProUGUI ammoCounter;
    // Start is called before the first frame update
    void Start()
    {
        //calls the values from the instantiated Player
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            instantiateManager = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>();
            pauseMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {

        
            //calls the weaponID of the equipped weapon
            PlayerEquip playerEquip = instantiateManager.playerEquip;

            if (playerEquip.equip == true)
            {
                clipCounter.gameObject.SetActive(true);
                ammoCounter.gameObject.SetActive(true);

                WeaponSystem weaponData = instantiateManager.playerEquip.weaponScript;

                clipCounter.text = "Clip: " + weaponData.currentClip + "/" + weaponData.maxClip;
                ammoCounter.text = "Ammo: " + weaponData.currentAmmo + "/" + weaponData.maxAmmo;
            }
            else
            {
                clipCounter.gameObject.SetActive(false);
                ammoCounter.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    pauseMenu.SetActive(true);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;


                    Time.timeScale = 0;

                    isPaused = true;
                }
                else
                {
                    Resume();
                }
           
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Resume();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Loadlevel(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void RestartLevel()
    {
        Loadlevel(SceneManager.GetActiveScene().buildIndex);

        pauseMenu.SetActive(false);

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }
}
