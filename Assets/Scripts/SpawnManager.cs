using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Transform enemySpawnPoints;
    [SerializeField] private Transform playerSpawnPoints;

    private LevelManager levelManager;


    private void Start()
    {
        levelManager = LevelManager.Instance;
        levelManager.OnCreateEnemyAction += LevelManager_OnCreateEnemyAction;
        levelManager.OnCreatePlayerAction += LevelManager_OnCreatePlayerAction;
    }

    private void LevelManager_OnCreatePlayerAction()
    {
        Instantiate(playerPrefab, playerSpawnPoints.position, Quaternion.identity);
    }

    private void LevelManager_OnCreateEnemyAction()
    {
        Instantiate(enemyPrefab, enemySpawnPoints.position, Quaternion.identity);
    }

}
