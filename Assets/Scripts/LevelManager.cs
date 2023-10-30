using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _Instance;
    public static LevelManager Instance
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

    [SerializeField] private List<LevelData> levelList;
    [SerializeField] private LevelData currentLevel;

    public event Action OnCreateEnemyAction;
    public event Action OnCreatePlayerAction;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void OnEnable()
    {
        EventBus.PlayerDeath += EventBus_PlayerDeath;
        EventBus.EnemyDeath += EventBus_EnemyDeath;
    }

    private void Start()
    {
        currentLevel = levelList[0];
    }

    private void EventBus_EnemyDeath(EnemyController obj)
    {
        if (currentLevel.enemyCount > 0)
        {
            OnCreateEnemyAction?.Invoke();
            currentLevel.enemyCount--;
        }
    }

    private void EventBus_PlayerDeath(PlayerController obj)
    {
        if (currentLevel.playerLifeCount > 0)
        {
            OnCreatePlayerAction?.Invoke();
            currentLevel.playerLifeCount--;
        }
    }
    private void OnDisable()
    {
        EventBus.PlayerDeath -= EventBus_PlayerDeath;
        EventBus.EnemyDeath -= EventBus_EnemyDeath;
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
