using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public static MapHandler Instance;

    [SerializeField] private WallTypes[] defaultWallMap;
    [SerializeField] private LevelSettings consturctedLevelSettings;
    [SerializeField] private WallTypes selectedWallType;
    [SerializeField] private GameObject gridMapConstructorPrefab;
    [SerializeField] private LayerMask areaLayer;
    [SerializeField] private LayerMask wallLayer;

    private GameManager gameManager;
    private GridMapConstructor gridMapConstructor;
    private MapConstructorUI mapConstructorUI;


    public event Action<LevelSettings> OnUpdateLevelSettingsUIAction;
    //public LevelSettings MapSettings { get => mapSettings; set => mapSettings = value; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        mapConstructorUI = GetComponent<MapConstructorUI>();
        mapConstructorUI.OnEnemyCountChangedAction += MapConstructorUI_OnEnemyCountChangedAction;
        mapConstructorUI.OnLevelIDChangedAction += MapConstructorUI_OnLevelIDChangedAction;
        mapConstructorUI.OnPlayerLifeChangedAction += MapConstructorUI_OnPlayerLifeChangedAction;
        mapConstructorUI.OnTrySaveMapAction += MapConstructorUI_OnTrySaveMapAction; 
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
            if (selectedWallType != WallTypes.Empty)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, areaLayer))
                {
                    Debug.Log("hit.name: " + hit.transform.gameObject);
                    Vector3 worldPosition = hit.point;
                    Vector3Int cellPosition = gridMapConstructor.grid.WorldToCell(worldPosition);
                    cellPosition.x /= 2;
                    cellPosition.z /= 2;
                    cellPosition.y = 1;
                    gridMapConstructor.TryPutObjectToSelectedCellPosition(cellPosition, selectedWallType);
                    Debug.Log("cellPosition: " + cellPosition);
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity ,wallLayer))
                {
                    Debug.Log("hit.name: " + hit.transform.gameObject.layer);
                    Vector3 worldPosition = hit.point;
                    Vector3Int cellPosition = gridMapConstructor.grid.WorldToCell(worldPosition);
                    GameObject wall = hit.transform.gameObject;
                    cellPosition.x /= 2;
                    cellPosition.z /= 2;
                    cellPosition.y = 1;
                    gridMapConstructor.RemoveObjectFromCell(cellPosition, wall);
                }
            }
            

        }
    }

    public void SetLevelSetting(LevelSettings storedMapSetting = null)
    {
        gridMapConstructor = Instantiate(gridMapConstructorPrefab, new Vector3(-4.5f, 0, 0),
                                 Quaternion.identity).GetComponent<GridMapConstructor>();
        if (storedMapSetting == null)
        {
            consturctedLevelSettings = new LevelSettings();
            WallTypes[] copiedMap = new WallTypes[defaultWallMap.Length];
            Array.Copy(defaultWallMap, copiedMap, defaultWallMap.Length);
            
            consturctedLevelSettings.wallMap = copiedMap;
            gridMapConstructor.wallMap = copiedMap;
        }
        else
        {
            consturctedLevelSettings = storedMapSetting;
            gridMapConstructor.wallMap = consturctedLevelSettings.wallMap;

        }
        OnUpdateLevelSettingsUIAction?.Invoke(consturctedLevelSettings);
    }

    private void MapConstructorUI_OnSelectedWallTypeChangedAction(WallTypes wallType)
    {
        selectedWallType = wallType;
    }

    private void MapConstructorUI_OnPlayerLifeChangedAction(int value)
    {
        consturctedLevelSettings.playerLifeCount = value;
    }

    private void MapConstructorUI_OnLevelIDChangedAction(int value)
    {
        consturctedLevelSettings.levelID = value;
    }

    private void MapConstructorUI_OnEnemyCountChangedAction(int value, EnemyType enemyType)
    {
        if (consturctedLevelSettings.enemyList.Contains(enemyType))
        {
            consturctedLevelSettings.enemyList.RemoveAll(enemy => enemyType == enemy);
        }

        for (int i = 0; i < value; i++)
        {
            consturctedLevelSettings.enemyList.Add(enemyType);
        }
    }
    private void OnDisable()
    {
        Destroy(gridMapConstructor.gameObject);
    }

    public bool CheckIsMapCorret()
    {
        int eagleCounter = 0;
        int playerSpawnCounter = 0;
        int enemySpawnCounter = 0;

        for (int i = 0; i < consturctedLevelSettings.wallMap.Length; i++)
        {
            if (consturctedLevelSettings.wallMap[i] == WallTypes.Eagle)
            {
                eagleCounter++;
            }
            else if (consturctedLevelSettings.wallMap[i] == WallTypes.PlayerSpawn)
            {
                playerSpawnCounter++;
            }
            else if (consturctedLevelSettings.wallMap[i] == WallTypes.EnemySpawn)
            {
                enemySpawnCounter++;
            }
        }

        if (eagleCounter != 1)
        {
            mapConstructorUI.UpdateFeedBackText(false, SaveConditions.EagleCount);
            return false;
        }
        else if (playerSpawnCounter != 1)
        {
            mapConstructorUI.UpdateFeedBackText(false, SaveConditions.PlayerSpawn);
            return false;
        }
        else if (enemySpawnCounter <= 0 || enemySpawnCounter > 3)
        {
            mapConstructorUI.UpdateFeedBackText(false, SaveConditions.EnemySpawn);
            return false;
        }
        else if (consturctedLevelSettings.enemyList.Count <= 0)
        {
            mapConstructorUI.UpdateFeedBackText(false, SaveConditions.EnemyCount);
            return false;
        }
        else
        {
            SaveMap();
            return true;
        }
    }
    private void MapConstructorUI_OnTrySaveMapAction()
    {
        CheckIsMapCorret();
    }
    public void SaveMap()
    {
        Map savedMap = new Map();
        savedMap.wallMap = consturctedLevelSettings.wallMap;
        LevelSettings levelCopy = consturctedLevelSettings.CopyData();

        if (gameManager.gameData.constructedLevelDataDic.ContainsKey(levelCopy.levelID))
        {
            gameManager.gameData.constructedLevelDataDic[levelCopy.levelID] = levelCopy;
        }
        else
        {
            gameManager.gameData.constructedLevelDataDic.Add(levelCopy.levelID, levelCopy);
        }
        mapConstructorUI.UpdateFeedBackText(true);
    }
}
