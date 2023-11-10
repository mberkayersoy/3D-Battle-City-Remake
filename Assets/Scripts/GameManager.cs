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
    public GameData gameData;
    private UIManager uiManager;
    public LevelManager CurrentLevelManager { get => currentLevelManager; }

    private void Start()
    {
        gameData = GameData.LoadGameData();
        constructedLevelDic = gameData.constructedLevelDataDic;
        
        uiManager = UIManager.Instance;
        uiManager.OnClickMenuAction += UiManager_OnClickMenuAction;
        uiManager.OnClickNextLevelAction += UiManager_OnClickNextLevelAction;
        uiManager.OnClickRestartLevelAction += UiManager_OnClickRestartLevelAction;
        EventBus.OnLevelSelectedAction += EventBus_OnLevelSelectedAction;
        EventBus.OnLevelEndAction += EventBus_OnLevelEndAction;
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
        PrepareTheLevel(e.selectedLevel);
        gameData.SetCurrentLevel(e.selectedLevel);
    }

    private void UiManager_OnClickRestartLevelAction()
    {
        gameData.SetCurrentLevel(gameData.GetCurrentLevel());
        PrepareTheLevel(gameData.GetCurrentLevel());
    }

    private void UiManager_OnClickNextLevelAction()
    {
        gameData.SetCurrentLevel(gameData.GetCurrentLevel() + 1);
        PrepareTheLevel(gameData.GetCurrentLevel());
    }

    private void UiManager_OnClickMenuAction()
    {
        CleanLevel();
    }

    private void CleanLevel()
    {
        Destroy(currentLevelManager.gameObject);
        currentLevelManager = null;
    }

    private void PrepareTheLevel(int level)
    {
        if (currentLevelManager != null)
        {
            CleanLevel();
        }

        currentLevelManager = Instantiate(levelManagerPrefab);
        LevelSettings levelCopy = defaultLevelList[level].CopyData();
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
    public List<EnemyType> enemyList = new List<EnemyType>();
    public int playerLifeCount;
    public MapSO mapSO;
    public WallTypes[] wallMap = new WallTypes[15 * 15];
    public LevelSettings CopyData()
    {
        LevelSettings copy = new LevelSettings();
        copy.levelID = levelID;
        copy.playerLifeCount = playerLifeCount;
        copy.enemyList = new List<EnemyType>(enemyList);
        if (mapSO != null)
        {
            copy.mapSO = mapSO;
        }
        else 
        {
            copy.wallMap = wallMap;
        }

        return copy;
    }
}
