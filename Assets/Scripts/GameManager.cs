using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            return _Instance;
        }

        private set
        {
            _Instance = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    [SerializeField] private List<LevelSettings> defaultLevelList;
    [SerializeField] private Dictionary<int,LevelSettings> constructedLevelDic;
    [SerializeField] private LevelManager levelManagerPrefab;
    [SerializeField] private LevelManager currentLevelManager;
    [SerializeField] private bool playingDefaultLevel;
    public GameData gameData;
    private UIManager uiManager;
    public LevelManager CurrentLevelManager { get => currentLevelManager; }
    public bool PlayingDefaultLevel { get => playingDefaultLevel; private set => playingDefaultLevel = value; }

    private void Start()
    {
        gameData = GameData.LoadGameData();
        constructedLevelDic = gameData.constructedLevelDataDic;
        
        uiManager = UIManager.Instance;
        uiManager.OnClickMenuAction += UiManager_OnClickMenuAction;
        uiManager.OnClickNextLevelAction += UiManager_OnClickNextLevelAction;
        uiManager.OnClickRestartLevelAction += UiManager_OnClickRestartLevelAction;
        EventBus.OnLevelSelectedAction += EventBus_OnLevelSelectedAction;
        EventBus.OnConsturctedLevelSelectedAction += EventBus_OnConsturctedLevelSelectedAction;
        EventBus.OnDefaultLevelEndAction += EventBus_OnLevelEndAction;
    }

    private void EventBus_OnConsturctedLevelSelectedAction(object sender, EventBus.OnLevelSelectedEventArgs e)
    {
        PrepareConstructedLevel(e.selectedLevel);
    }

    private void EventBus_OnLevelEndAction(object sender, EventBus.OnLevelEndEventArgs e)
    {
        if (e.isSuccess)
        {
            gameData.SetActiveMaxLevel();
        }
    }

    private void EventBus_OnLevelSelectedAction(object sender, EventBus.OnLevelSelectedEventArgs e)
    {
        PrepareDefaultLevel(e.selectedLevel);
        gameData.SetCurrentLevel(e.selectedLevel);
    }

    private void UiManager_OnClickRestartLevelAction()
    {
        gameData.SetCurrentLevel(gameData.GetCurrentLevel());
        PrepareDefaultLevel(gameData.GetCurrentLevel());
    }

    private void UiManager_OnClickNextLevelAction()
    {
        gameData.SetCurrentLevel(gameData.GetCurrentLevel() + 1);
        PrepareDefaultLevel(gameData.GetCurrentLevel());
    }

    private void UiManager_OnClickMenuAction()
    {
        CleanLevel();
    }

    private void CleanLevel()
    {
        ObjectPool.Instance.DeactivateAllObjects();
        Destroy(currentLevelManager.gameObject);
        currentLevelManager = null;
    }

    private void PrepareDefaultLevel(int level)
    {
        playingDefaultLevel = true;
        if (currentLevelManager != null)
        {
            CleanLevel();
        }

        currentLevelManager = Instantiate(levelManagerPrefab);
        LevelSettings levelCopy = defaultLevelList[level - 1].CopyData();
        currentLevelManager.CopyLevelData(levelCopy);
    }

    private void PrepareConstructedLevel(int level)
    {
        playingDefaultLevel = false;
        if (currentLevelManager != null)
        {
            CleanLevel();
        }

        currentLevelManager = Instantiate(levelManagerPrefab);
        LevelSettings levelCopy = constructedLevelDic[level].CopyData();
        currentLevelManager.CopyLevelData(levelCopy);
    }

    private void OnDestroy()
    {
        GameData.SaveGameData(gameData);
    }
}

[System.Serializable]
public class LevelSettings
{
    public int levelID;
    public int score;
    public int playerLifeCount;
    public bool isDefaultMap;
    public List<EnemyType> enemyList = new List<EnemyType>();
    public WallTypes[] wallMap = new WallTypes[15 * 15];
    public LevelSettings CopyData()
    {
        LevelSettings copy = new LevelSettings();
        copy.levelID = levelID;
        copy.playerLifeCount = playerLifeCount;
        copy.enemyList = new List<EnemyType>(enemyList);
        copy.isDefaultMap = isDefaultMap;
        copy.wallMap = new WallTypes[wallMap.Length];
        Array.Copy(wallMap, copy.wallMap, wallMap.Length);
        
        return copy;
    }
}
