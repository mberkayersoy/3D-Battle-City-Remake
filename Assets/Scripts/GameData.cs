using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int availableLevelCount;
    public int currentLevel;
    // other variables...

    public GameData(int maxAvailableLevelCount, int currentLevel)
    {
        this.availableLevelCount = maxAvailableLevelCount;
        this.currentLevel = currentLevel;
    }
    public int GetMaxAvailableLevelCount()
    {
        Debug.Log("maxAvailableLevelCount: " + availableLevelCount);
        return availableLevelCount;
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
        if (currentLevel > availableLevelCount)
        {
            availableLevelCount = currentLevel;
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public static void SaveGameData(GameData gameData)
    {
        string jsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + "/GameData.json";
        File.WriteAllText(filePath, jsonData);
    }
    public static GameData LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/GameData.json";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            // Eðer dosya yoksa boþ bir PlayerData nesnesi döndürebilirsiniz.
            return new GameData(1, 0);
        }
    }
}
