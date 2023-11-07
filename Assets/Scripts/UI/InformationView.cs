using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationView : MonoBehaviour
{
    [SerializeField] private GameObject enemyPic;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private TextMeshProUGUI playerLifeCountText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    private LevelManager levelManager;

    private void OnEnable()
    {
        EventBus.OnEnemyDeathAction += EventBus_OnEnemyDeathAction;
        EventBus.OnPlayerDeathAction += EventBus_OnPlayerDeathAction;
    }

    private void EventBus_OnPlayerDeathAction(PlayerController obj)
    {
        UpdatePlayerLifeVisual();
    }

    private void EventBus_OnEnemyDeathAction(EnemyController obj)
    {
        UpdateEnemyCountVisual();
    }

    public void SetInformation(LevelManager currentLevelManager)
    {
        levelManager = currentLevelManager;
        levelManager.OnScoreChangeAction += LevelManager_OnScoreChangeAction;
        if (currentLevelManager == null)
        {
            Debug.LogError("levelmanager is Null");
        }
        UpdateEnemyCountVisual();

        levelText.text = "Level: " + levelManager.CurrentLevel.levelID.ToString();
        playerLifeCountText.text = "x" + levelManager.CurrentLevel.playerLifeCount;
        playerScoreText.text = "0";
    }

    private void LevelManager_OnScoreChangeAction()
    {
        UpdateScoreVisual();
    }

    private void UpdateScoreVisual()
    {
        playerScoreText.text = levelManager.LevelScore.ToString();
    }

    private void UpdateEnemyCountVisual()
    {
        foreach (Transform child in enemyContainer)
        {
            if (child == enemyContainer) continue;
            Destroy(child.gameObject);
        }
        for (int i = 0; i < levelManager.CurrentLevel.enemyList.Count; i++)
        {
            EnemyTypeInfo enemyTypeInfo = Instantiate(enemyPic, enemyContainer).GetComponent<EnemyTypeInfo>();
            enemyTypeInfo.SetEnemyType(levelManager.CurrentLevel.enemyList[i]);
        }
    }

    private void UpdatePlayerLifeVisual()
    {
        playerLifeCountText.text = "x" + levelManager.CurrentLevel.playerLifeCount.ToString();
    }

    private void OnDisable()
    {
        EventBus.OnEnemyDeathAction -= EventBus_OnEnemyDeathAction;
        EventBus.OnPlayerDeathAction -= EventBus_OnPlayerDeathAction;

        if (levelManager == null)
        {
            Debug.Log("levelmanager null oldugu icin cikart.");
            levelManager.OnScoreChangeAction -= LevelManager_OnScoreChangeAction;

        }
    }

}
