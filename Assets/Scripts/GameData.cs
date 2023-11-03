using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class GameData
{
    public int availableLevelCount;
    public int currentLevel;
    public Dictionary<int,LevelData> levelDataDic;

    public void AddLevelData(LevelData leveldata)
    {
        levelDataDic.Add(leveldata.levelID, leveldata);
    }

    public void UpdateLevelData(LevelData newLevelData)
    {
        levelDataDic[newLevelData.levelID] = newLevelData;
    }
    public GameData(int maxAvailableLevelCount, int currentLevel, Dictionary<int,LevelData> levelDataDic)
    {
        this.availableLevelCount = maxAvailableLevelCount;
        this.currentLevel = currentLevel;
        this.levelDataDic = levelDataDic;

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
            AddLevelData(new LevelData(currentLevel, 0));
            availableLevelCount = currentLevel;
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public static void SaveGameData(GameData gameData)
    {
        string filePath = Application.persistentDataPath + "/GameData.json";

        string json = JsonConvert.SerializeObject(gameData);
        File.WriteAllText(filePath, json);
    }
    public static GameData LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/GameData.json";
        Debug.Log("filePath: " + filePath);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GameData>(json);
        }
        else
        {
            Debug.Log("file not Exist");
            Dictionary<int, LevelData> firstLevelDataList = new Dictionary<int, LevelData>();
            firstLevelDataList.Add(0, new LevelData(0, 0));
            return new GameData(0, 0, firstLevelDataList);
        }
    }
}

[System.Serializable]
public class LevelData
{
    public int levelID;
    public int levelScore;

    public LevelData(int levelID, int levelScore)
    {
        this.levelID = levelID;
        this.levelScore = levelScore;
    }
}
