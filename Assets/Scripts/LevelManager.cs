using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int levelScore;

    [Header("GAME OBJECTS")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject gridMapPrefab;
    [SerializeField] private GameObject abilityTestObject;

    [Header("REFERENCES")]
    [SerializeField] private LevelSettings currentLevel = new LevelSettings();
    [SerializeField] private GridMap currentGridMap;

    [Header("TRANSFORMS")]
    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] private Transform playerSpawnPoints;
    [SerializeField] private Transform enemyMainTarget;
    [SerializeField] private float enemySpawnTimeOut;
    [SerializeField] private float remainingEnemySpawnTimeOut;
    [SerializeField] private int remainingEnemyCount;
    public event Action OnScoreChangeAction;

    private GameManager gameManager;
    public List<Transform> EnemySpawnPoints { get => enemySpawnPoints; set => enemySpawnPoints = value; }
    public Transform PlayerSpawnPoints { get => playerSpawnPoints; set => playerSpawnPoints = value; }
    public Transform EnemyMainTarget { get => enemyMainTarget; set => enemyMainTarget = value; }
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
    private void Start()
    {
        GameObject gridMapInstance = Instantiate(gridMapPrefab, new Vector3(0, -1, 0), Quaternion.identity, transform);
        currentGridMap = gridMapInstance.GetComponent<GridMap>();
        currentGridMap.currentMap = currentLevel.mapSO;
        UIManager.Instance.OnStartSpawnAction += UIManager_OnStartSpawnAction;
        //CopyLevelData();
        remainingEnemySpawnTimeOut = enemySpawnTimeOut;
    }

    private void Update()
    {
        //InstantiateAbility();

        if (currentLevel.enemyList.Count > 0)
        {
            remainingEnemySpawnTimeOut -= Time.deltaTime;
            if (remainingEnemySpawnTimeOut < 0)
            {
                InstantiateEnemy();
            }
        }
    }
    public void CopyLevelData(LevelSettings levelSettings)
    {
        currentLevel.levelID = levelSettings.levelID;
        currentLevel.playerLifeCount = levelSettings.playerLifeCount;
        currentLevel.enemyList = new List<EnemyType>(levelSettings.enemyList);
        currentLevel.mapSO = levelSettings.mapSO;
        remainingEnemyCount = levelSettings.enemyList.Count;
    }
    private void EventBus_OnScoreUpdateAction(object sender, EventBus.OnScoreUpdateEventArgs e)
    {
        levelScore += e.addScore;
        OnScoreChangeAction?.Invoke(); // for InformationView class.
    }

    public void UIManager_OnStartSpawnAction()
    {
        InstantiateEnemy();
        Instantiate(playerPrefab, playerSpawnPoints.position, Quaternion.identity, transform);
    }

    private void EventBus_EnemyDeath(EnemyController obj)
    {
        remainingEnemyCount--;
        if (remainingEnemyCount <= 0)
        {
            //InstantiateEnemy();
            SetLevelEndDatas(true);
        }
        //else
        //{

        //}
    }

    private void InstantiateEnemy()
    {
        int randomEnemyIndex = UnityEngine.Random.Range(0, currentLevel.enemyList.Count);
        EnemyController newEnemyObject = Instantiate(enemyPrefab, enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Count)].position,
            Quaternion.identity, transform).GetComponent<EnemyController>();
        newEnemyObject.SetEnemyCharacteristics(currentLevel.enemyList[randomEnemyIndex]);
        currentLevel.enemyList.RemoveAt(randomEnemyIndex);
        remainingEnemySpawnTimeOut = enemySpawnTimeOut;
    }

    private void SetLevelEndDatas(bool isSucces)
    {
        LevelData newLevelData = new LevelData(currentLevel.levelID, levelScore);

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

                if (gameManager.gameData.currentLevelID == gameManager.gameData.activeMaxLevelID)
                {
                    // Unlock Next level
                    gameManager.gameData.AddLevelData(new LevelData(newLevelData.levelID + 1, 0));
                }

            }
            else
            {
                // Add played level data
                gameManager.gameData.AddLevelData(newLevelData);
                // Unlock Next level
                //gameManager.gameData.AddLevelData(new LevelData(newLevelData.levelID + 1, 0));
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

    public void InstantiateAbility()
    {
        int randomIndex = UnityEngine.Random.Range(0, currentGridMap.emptyCellPositions.Count);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(abilityTestObject, currentGridMap.emptyCellPositions[randomIndex] + Vector3.up * 3, Quaternion.identity, transform);
        }
    }

    private void OnDisable()
    {
        EventBus.OnPlayerDeathAction -= EventBus_PlayerDeath;
        EventBus.OnEnemyDeathAction -= EventBus_EnemyDeath;
        UIManager.Instance.OnStartSpawnAction -= UIManager_OnStartSpawnAction;
    }
}

public enum AbilityType
{
    Grenade
}

