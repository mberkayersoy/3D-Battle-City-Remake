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

    public LevelManager CurrentLevelManager { get => currentLevelManager;}

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

    [SerializeField] private LevelManager levelManagerPrefab;
    [SerializeField] private LevelManager currentLevelManager;
    public GameData gameData;
    private UIManager uiManager;

    private void Start()
    {
        gameData = GameData.LoadGameData();

        Debug.Log("currentlevle: " + gameData.GetCurrentLevel());
        Debug.Log("maxlevel: " + gameData.GetMaxAvailableLevelCount());
        
        uiManager = UIManager.Instance;
        uiManager.OnClickMenuAction += UiManager_OnClickMenuAction;
        uiManager.OnClickNextLevelAction += UiManager_OnClickNextLevelAction;
        uiManager.OnClickRestartLevelAction += UiManager_OnClickRestartLevelAction;
        EventBus.OnLevelSelectedAction += EventBus_OnLevelSelectedAction;
    }

    private void EventBus_OnLevelSelectedAction(object sender, EventBus.OnLevelSelectedEventArgs e)
    {
        PrepareTheLevel(e.selectedLevel);
        gameData.SetCurrentLevel(e.selectedLevel);
    }

    private void UiManager_OnClickRestartLevelAction()
    {
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
           Debug.Log("if");
           CleanLevel();
           currentLevelManager = Instantiate(levelManagerPrefab);
           currentLevelManager.CurrentLevel = currentLevelManager.LevelList[level];
           Debug.Log(currentLevelManager.name);
        }
        else
        {
            currentLevelManager = Instantiate(levelManagerPrefab);
            currentLevelManager.CurrentLevel = currentLevelManager.LevelList[level];
        }
    }

    private void OnDestroy()
    {
        GameData.SaveGameData(gameData);
    }
}
