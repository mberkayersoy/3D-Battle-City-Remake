using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    //private static LevelManager _Instance;
    //public static LevelManager Instance
    //{
    //    get
    //    {
    //        return _Instance;
    //    }

    //    private set
    //    {
    //        _Instance = value;
    //    }
    //}

    [SerializeField] private GameObject gridMapPrefab;
    [SerializeField] private GridMap currentGridMap;
    [SerializeField] private List<LevelData> levelList;
    [SerializeField] private LevelData currentLevel;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] private Transform playerSpawnPoints;
    [SerializeField] private Transform enemyMainTarget;

    public List<Transform> EnemySpawnPoints { get => enemySpawnPoints; set => enemySpawnPoints = value; }
    public Transform PlayerSpawnPoints { get => playerSpawnPoints; set => playerSpawnPoints = value; }
    public Transform EnemyMainTarget { get => enemyMainTarget; set => enemyMainTarget = value; }
    public List<LevelData> LevelList { get => levelList; set => levelList = value; }
    public LevelData CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public GridMap CurrentGridMap { get => currentGridMap; set => currentGridMap = value; }

    //private void Awake()
    //{
    //    if (Instance != null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    Instance = this;
    //}
    private void OnEnable()
    {
        EventBus.OnPlayerDeathAction += EventBus_PlayerDeath;
        EventBus.OnEnemyDeathAction += EventBus_EnemyDeath;
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
            EventBus.PublishLevelEnd(this, true);
        }
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
            EventBus.PublishLevelEnd(this, false);
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
public class LevelData
{
    public int level;
    public int enemyCount;
    public int numberOfEnemyAtOnce;
    public int playerLifeCount;
    public MapSO mapSO;
}
