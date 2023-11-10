using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public static MapHandler Instance;
    [SerializeField] private WallTypes[] defaultWallMap;
    private MapConstructorUI mapConstructorUI;
    [SerializeField] private LevelSettings mapSettings;
    [SerializeField] private WallTypes selectedWallType;
    [SerializeField] private GameObject gridMapConstructorPrefab;
    [SerializeField] private LayerMask constructAreaLayer;
    private GridMapConstructor gridMapConstructor;

    public LevelSettings MapSettings { get => mapSettings; set => mapSettings = value; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        mapConstructorUI = GetComponent<MapConstructorUI>();
        mapConstructorUI.OnEnemyCountChangedAction += MapConstructorUI_OnEnemyCountChangedAction;
        mapConstructorUI.OnLevelIDChangedAction += MapConstructorUI_OnLevelIDChangedAction;
        mapConstructorUI.OnPlayerLifeChangedAction += MapConstructorUI_OnPlayerLifeChangedAction;
        mapConstructorUI.OnSaveMapAction += MapConstructorUI_OnClickSaveMapAction;
        mapConstructorUI.OnSelectedWallTypeChangedAction += MapConstructorUI_OnSelectedWallTypeChangedAction;
    }

    private void Update()
    {
        GetSelectedGridCellPosition();
    }
    public void GetSelectedGridCellPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, constructAreaLayer))
            {
                Vector3 worldPosition = hit.point;
                Vector3Int cellPosition = gridMapConstructor.grid.WorldToCell(worldPosition);
                cellPosition.x /= 2; 
                cellPosition.z /= 2;
                cellPosition.y = 1;
                gridMapConstructor.TryPutObjectToSelectedCellPosition(cellPosition, selectedWallType);
                Debug.Log("cellPosition: " + cellPosition);


                /* To do: 
                 * I will use below comments another function.
                 * Instead of scriptable object I will use json to store and update map data.
                 * Because Scriptable object changing not recommended in run time.
                */
                // update wallMap array
                //int cellIndex = cellPosition.z * defaultMapSO.width + cellPosition.x;
                //defaultMapSO.wallMap[cellIndex] = selectedWallType;
            }
        }
    }

    public void SetLevelSetting(LevelSettings storedMapSetting = null)
    {
        gridMapConstructor = Instantiate(gridMapConstructorPrefab, new Vector3(-4.5f, 0, 0),
                                 Quaternion.identity).GetComponent<GridMapConstructor>();
        if (storedMapSetting == null)
        {
            mapSettings = new LevelSettings();
            WallTypes[] copiedMap = new WallTypes[defaultWallMap.Length];
            Array.Copy(defaultWallMap, copiedMap, defaultWallMap.Length);
            
            mapSettings.wallMap = copiedMap;
            gridMapConstructor.consturctedMap.wallMap = copiedMap;
        }
        else
        {
            mapSettings = storedMapSetting;
            gridMapConstructor.consturctedMap.wallMap = mapSettings.wallMap;
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
    private void MapConstructorUI_OnClickSaveMapAction()
    {
        gridMapConstructor.CheckIsMapCorret();
    }

    private void OnDisable()
    {
        Destroy(gridMapConstructor.gameObject);
    }

}
