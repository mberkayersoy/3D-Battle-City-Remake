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
    public Dictionary<int, SavedData> defaultLevelDataDic;
    public Dictionary<int,LevelSettings> constructedLevelDataDic;

    public void AddLevelData(SavedData leveldata)
    {
        defaultLevelDataDic.Add(leveldata.levelID, leveldata);
    }

    public void UpdateLevelData(SavedData newLevelData)
    {
        defaultLevelDataDic[newLevelData.levelID] = newLevelData;
    }
    public GameData(int activeMaxLevelID, int currentLevelID, Dictionary<int, SavedData> defaultLevelDataDic, Dictionary<int, LevelSettings> constructedLevelDataDic)
    {
        this.activeMaxLevelID = activeMaxLevelID;
        this.currentLevelID = currentLevelID;
        this.defaultLevelDataDic = defaultLevelDataDic;
        this.constructedLevelDataDic = constructedLevelDataDic;
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
            Dictionary<int, SavedData> firstLevelDataList = new Dictionary<int, SavedData>();
            firstLevelDataList.Add(1, new SavedData(1, 0));
            return new GameData(1, 1, firstLevelDataList, new Dictionary<int, LevelSettings>());
        }
    }
}

[System.Serializable]
public class SavedData
{
    public int levelID;
    public int levelScore;

    public SavedData(int levelID, int levelScore)
    {
        this.levelID = levelID;
        this.levelScore = levelScore;
    }
}
