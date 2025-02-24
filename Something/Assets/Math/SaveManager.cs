using UnityEngine;
using System.IO;

public class SaveManager
{
    private string saveFilePath;

    public SaveManager()
    {
        saveFilePath = Application.persistentDataPath + "/savefile.json";
    }

    public void SaveGame(GameState gameState)
    {
        string json = JsonUtility.ToJson(gameState);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved to " + saveFilePath);
    }

    public GameState LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameState gameState = JsonUtility.FromJson<GameState>(json);
            Debug.Log("Game Loaded from " + saveFilePath);
            return gameState;
        }
        else
        {
            Debug.LogWarning("No save file found at " + saveFilePath);
            return null;
        }
    }
}
