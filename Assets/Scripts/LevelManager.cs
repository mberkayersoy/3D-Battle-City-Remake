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

    [SerializeField] private float enemySpawnTimeOut;
    [SerializeField] private float remainingEnemySpawnTimeOut;
    [SerializeField] private int remainingEnemyCount;
    public event Action OnScoreChangeAction;

    private GameManager gameManager;
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

        currentGridMap.wallMap = currentLevel.wallMap;
        

        UIManager.Instance.OnStartSpawnAction += UIManager_OnStartSpawnAction;

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

        currentLevel.wallMap = levelSettings.wallMap;
        
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
        Instantiate(playerPrefab, currentGridMap.playerSpawnTransform.position, Quaternion.identity, transform);
    }

    private void EventBus_EnemyDeath(EnemyController obj)
    {
        remainingEnemyCount--;
        if (remainingEnemyCount <= 0)
        {
            //InstantiateEnemy();
            SetLevelEndDatas(true);
        }
    }

    private void InstantiateEnemy()
    {
        if (currentGridMap != null)
        {
            int randomEnemyIndex = UnityEngine.Random.Range(0, currentLevel.enemyList.Count);
            EnemyController newEnemyObject = Instantiate(enemyPrefab, currentGridMap.enemySpawnTransform[UnityEngine.Random.Range(0, currentGridMap.enemySpawnTransform.Count)].position,
                                             Quaternion.Euler(0f, 180f, 0f), transform).GetComponent<EnemyController>();

            newEnemyObject.SetEnemyCharacteristics(currentLevel.enemyList[randomEnemyIndex]);
            currentLevel.enemyList.RemoveAt(randomEnemyIndex);
            remainingEnemySpawnTimeOut = enemySpawnTimeOut;
        }
    }

    private void SetLevelEndDatas(bool isSucces)
    {
        if (gameManager.PlayingDefaultLevel)
        {
            SavedData newLevelData = new SavedData(currentLevel.levelID, levelScore);

            // Level successfully finished.
            if (isSucces)
            {
                // if this level played before or unlock.
                if (gameManager.gameData.defaultLevelDataDic.ContainsKey(currentLevel.levelID))
                {
                    // if stored levelscore < new score. Update stored data.
                    if (gameManager.gameData.defaultLevelDataDic[currentLevel.levelID].levelScore < newLevelData.levelScore)
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
                        gameManager.gameData.AddLevelData(new SavedData(newLevelData.levelID + 1, 0));
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
                if (gameManager.gameData.defaultLevelDataDic.ContainsKey(currentLevel.levelID))
                {
                    if (gameManager.gameData.defaultLevelDataDic.TryGetValue(currentLevel.levelID, out SavedData storedLevelData))
                    {
                        if (storedLevelData.levelScore < newLevelData.levelScore)
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
            Debug.Log("DEFAULT LEVEL");
            EventBus.PublishDefaultLevelEnd(this, isSucces);
        }
        else
        {
            EventBus.PublishConstructedLevelEnd(this, isSucces);
        }


    }
    private void EventBus_PlayerDeath(PlayerController obj)
    {
        if (currentLevel.playerLifeCount > 0)
        {
            Instantiate(playerPrefab, currentGridMap.playerSpawnTransform.position, Quaternion.identity, transform);
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

