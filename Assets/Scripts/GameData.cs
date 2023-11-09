using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class GameData
{
    public int activeMaxLevelID;
    public int currentLevelID;
    public Dictionary<int,LevelData> defaultLevelDataDic;
    public Dictionary<int,LevelData> constructedLevelDataDic;

    public void AddLevelData(LevelData leveldata)
    {
        defaultLevelDataDic.Add(leveldata.levelID, leveldata);
    }

    public void UpdateLevelData(LevelData newLevelData)
    {
        defaultLevelDataDic[newLevelData.levelID] = newLevelData;
    }
    public GameData(int activeMaxLevelID, int currentLevelID, Dictionary<int,LevelData> levelDataDic)
    {
        this.activeMaxLevelID = activeMaxLevelID;
        this.currentLevelID = currentLevelID;
        this.defaultLevelDataDic = levelDataDic;

    }
    public int GetActiveMaxLevel()
    {
        Debug.Log("maxAvailableLevelCount: " + activeMaxLevelID);
        return activeMaxLevelID;
    }
    public void SetActiveMaxLevel()
    {
        if (currentLevelID == activeMaxLevelID)
        {
            activeMaxLevelID++;
        }
    }

    public void SetCurrentLevel(int level)
    {
        currentLevelID = level;
        //if (currentLevel < 0)
        //{
        //    currentLevel = 0;
        //}
    }

    public int GetCurrentLevel()
    {
        return currentLevelID;
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
