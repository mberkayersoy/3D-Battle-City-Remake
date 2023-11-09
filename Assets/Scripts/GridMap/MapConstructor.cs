using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConstructor : MonoBehaviour
{
    private MapConstructorUI mapConstructorUI;
    private MapSO defaultMapSO;
    [SerializeField] private LevelSettings mapSettings;
    [SerializeField] private WallTypes selectedWallType;

    private void Start()
    {
        mapConstructorUI = GetComponent<MapConstructorUI>();
        mapConstructorUI.OnEnemyCountChangedAction += MapConstructorUI_OnEnemyCountChangedAction;
        mapConstructorUI.OnLevelIDChangedAction += MapConstructorUI_OnLevelIDChangedAction;
        mapConstructorUI.OnPlayerLifeChangedAction += MapConstructorUI_OnPlayerLifeChangedAction;
        mapConstructorUI.OnSaveMapAction += MapConstructorUI_OnSaveMapAction;
        mapConstructorUI.OnSelectedWallTypeChangedAction += MapConstructorUI_OnSelectedWallTypeChangedAction;
    }

    public void SetLevelSetting(LevelSettings storedMapSetting = null)
    {
        if (storedMapSetting == null)
        {
            mapSettings = new LevelSettings();
        }
        else
        {
            mapSettings = storedMapSetting;
        }
    }

    private void MapConstructorUI_OnSelectedWallTypeChangedAction(WallTypes wallType)
    {
        selectedWallType = wallType;
    }

    private void MapConstructorUI_OnPlayerLifeChangedAction(int value)
    {
        mapSettings.playerLifeCount = value;
    }

    private void MapConstructorUI_OnLevelIDChangedAction(int value)
    {
        mapSettings.levelID = value;
    }

    private void MapConstructorUI_OnEnemyCountChangedAction(int value, EnemyType enemyType)
    {
        if (mapSettings.enemyList.Contains(enemyType))
        {
            mapSettings.enemyList.RemoveAll(enemy => enemyType == enemy);
        }

        for (int i = 0; i < value; i++)
        {
            mapSettings.enemyList.Add(enemyType);
        }

    }
    private void MapConstructorUI_OnSaveMapAction()
    {
        throw new System.NotImplementedException();
    }

}
