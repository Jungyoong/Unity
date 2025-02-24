using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen, addPlayer, levelScreen;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject victoryScreen;
    public GameObject warningPrompt;
    public TMP_Text enemyAmount;
    public CharacterSelection players;

    private SaveManager saveManager;
    public GameState currentGameState;

    public bool gameOver;
    public bool paused;

    [Header("Overall Scene Settings & Inputs")]
    public float direction;
    public bool canMoveForward;
    public Transform center;

    void Awake()
    {
        saveManager = new();
    }

    void Start()
    {
        GameState loadedGameState = saveManager.LoadGame();
        
        if (loadedGameState != null)
            currentGameState = loadedGameState;

        paused = false;
        gameOver = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !paused)
                Pause();
            else if (Input.GetKeyDown(KeyCode.Escape) && paused)
                Unpause();
        }
    }
    public void GameOver()
    {
        gameOver = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameOverScreen.SetActive(true);
    }
    public void Restart()
    {   
        pauseScreen.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        if (pauseScreen == null)
            return;
        paused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseScreen.SetActive(true);
    }

    public void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseScreen.SetActive(false);
    }

    public void Victory()
    {
        victoryScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        currentGameState.levelsDone = SceneManager.GetActiveScene().buildIndex;
        saveManager.SaveGame(currentGameState);
    }

    public void OpenScreen(GameObject open)
    {
        open.SetActive(!open.activeSelf);
    }

    public void LoadLevel(int scene)
    {
        if (players.players.Length < 1)
        {
            OpenScreen(warningPrompt);
            return;
        }

        if (currentGameState.levelsDone > scene - 1)
            SceneManager.LoadScene(scene + 1);
        else
            Debug.Log("no");
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
