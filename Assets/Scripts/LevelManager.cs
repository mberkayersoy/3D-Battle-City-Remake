using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int levelScore;

    [SerializeField] private GameObject gridMapPrefab;
    [SerializeField] private GridMap currentGridMap;
    [SerializeField] private List<LevelSettings> levelList;
    [SerializeField] private LevelSettings currentLevel;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] private Transform playerSpawnPoints;
    [SerializeField] private Transform enemyMainTarget;


    private GameManager gameManager;
    public List<Transform> EnemySpawnPoints { get => enemySpawnPoints; set => enemySpawnPoints = value; }
    public Transform PlayerSpawnPoints { get => playerSpawnPoints; set => playerSpawnPoints = value; }
    public Transform EnemyMainTarget { get => enemyMainTarget; set => enemyMainTarget = value; }
    public List<LevelSettings> LevelList { get => levelList; set => levelList = value; }
    public LevelSettings CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public GridMap CurrentGridMap { get => currentGridMap; set => currentGridMap = value; }
    public int LevelScore { get => levelScore; private set => levelScore = value; }

    private void OnEnable()
    {
        EventBus.OnPlayerDeathAction += EventBus_PlayerDeath;
        EventBus.OnEnemyDeathAction += EventBus_EnemyDeath;
        EventBus.OnScoreUpdateAction += EventBus_OnScoreUpdateAction;
        gameManager = GameManager.Instance;
    }

    private void EventBus_OnScoreUpdateAction(object sender, EventBus.OnScoreUpdateEventArgs e)
    {
        levelScore += e.addScore;
        Debug.Log("updatedlevelscore: " + levelScore);
    }

    private void Start()
    {
        GameObject gridMapInstance = Instantiate(gridMapPrefab, new Vector3(0, -1, 0), Quaternion.identity, transform);
        currentGridMap = gridMapInstance.GetComponent<GridMap>();
        currentGridMap.currentMap = currentLevel.mapSO;
        UIManager.Instance.OnStartSpawnAction += UIManager_OnStartSpawnAction;
    }


    public void UIManager_OnStartSpawnAction()
    {
        Instantiate(enemyPrefab, enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Count)].position, Quaternion.identity, transform);
        Instantiate(playerPrefab, playerSpawnPoints.position, Quaternion.identity, transform);
    }

    private void EventBus_EnemyDeath(EnemyController obj)
    {
        if (currentLevel.enemyCount > 0)
        {
            Instantiate(enemyPrefab, enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Count)].position, Quaternion.identity, transform);
            currentLevel.enemyCount--;
        }
        else
        {
            SetLevelEndDatas(true);
        }
    }

    private void SetLevelEndDatas(bool isSucces)
    {

        // Every player life adds 1000 points.
        levelScore += currentLevel.playerLifeCount * 1000;
        LevelData newLevelData = new LevelData(currentLevel.levelID, levelScore);

        Debug.Log("new level-> " + newLevelData.levelID + "  " + newLevelData.levelScore);
        Debug.Log("new level-> " + newLevelData.levelID + "  " + levelScore);

        // Level successfully finished.
        if (isSucces)
        {


            // if this level played before or unlock.
            if (gameManager.gameData.levelDataDic.ContainsKey(currentLevel.levelID))
            {
                // if stored levelscore < new score. Update stored data.
                if (gameManager.gameData.levelDataDic[currentLevel.levelID].levelScore < newLevelData.levelScore)
                {
                    gameManager.gameData.UpdateLevelData(newLevelData);
                }
                else
                {
                    // if new score is not bigger than stored score. Dont do anything.
                }
                
            }
            else
            {
                // Add played level data
                gameManager.gameData.AddLevelData(newLevelData);
                // Unlock Next level
               // gameManager.gameData.AddLevelData(new LevelData(newLevelData.levelID + 1, 0));
            }
        }
        // Level failed.
        else
        {
            if (gameManager.gameData.levelDataDic.ContainsKey(currentLevel.levelID))
            {
                if (gameManager.gameData.levelDataDic.TryGetValue(currentLevel.levelID, out LevelData storedLevelData))
                {
                    if (storedLevelData.levelScore > newLevelData.levelScore)
                    {
                        gameManager.gameData.UpdateLevelData(newLevelData);
                    }
                    else
                    {
                        // if new score is not bigger than old score. Dont do anything.
                    }
                }
            }
        }

        gameManager.gameData.SetCurrentLevel(gameManager.gameData.GetCurrentLevel() + 1);
        EventBus.PublishLevelEnd(this, isSucces);
    }
    private void EventBus_PlayerDeath(PlayerController obj)
    {
        if (currentLevel.playerLifeCount > 0)
        {
            Instantiate(playerPrefab, playerSpawnPoints.position, Quaternion.identity, transform);
            currentLevel.playerLifeCount--;
        }
        else
        {
            SetLevelEndDatas(false);
        }
    }
    private void OnDisable()
    {
        EventBus.OnPlayerDeathAction -= EventBus_PlayerDeath;
        EventBus.OnEnemyDeathAction -= EventBus_EnemyDeath;
        UIManager.Instance.OnStartSpawnAction -= UIManager_OnStartSpawnAction;
    }
}

[Serializable]
public class LevelSettings
{
    public int levelID;
    public int enemyCount;
    public int numberOfEnemyAtOnce;
    public int playerLifeCount;
    public MapSO mapSO;
}
