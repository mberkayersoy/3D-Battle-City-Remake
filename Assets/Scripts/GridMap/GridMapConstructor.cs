using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapConstructor : MonoBehaviour
{
    public Grid grid;
    public Map consturctedMap;
    private const int width = 15;
    private const int height = 15;
    public GameObject groundCube;
    public GameObject border;
    public GameObject brickWall;
    public GameObject staticWall;
    public GameObject grassWall;
    public GameObject eagle;
    public GameObject spawnAreaPlayer;
    public GameObject spawnAreaEnemy;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }
    void Start()
    {
        ConstructGround(15, 15);
        ConstructMap();
    }

    public void ConstructGround(int width, int height)
    {
        // Create ground as one piece
        Vector3 centerPosition = Vector3.Lerp(grid.GetCellCenterWorld(new Vector3Int(0, 0, 0)), grid.GetCellCenterWorld(new Vector3Int(width * 2 - 1, 0, height * 2 - 1)), 0.5f);
        GameObject cell = Instantiate(groundCube, centerPosition, Quaternion.identity, gameObject.transform);
        cell.transform.localScale = new Vector3(width, 1, height);
    }

    public void ConstructMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (consturctedMap.wallMap[j + i * width] == WallTypes.Empty)
                {

                }
                else if (consturctedMap.wallMap[j + i * width] == WallTypes.Bricks)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), brickWall, true);
                }
                else if (consturctedMap.wallMap[j + i * width] == WallTypes.Border)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), border, false);
                }
                else if (consturctedMap.wallMap[j + i * width] == WallTypes.Static)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), staticWall, false);
                }
                else if (consturctedMap.wallMap[j + i * width] == WallTypes.Grass)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), grassWall, false);
                }
                else if (consturctedMap.wallMap[j + i * width] == WallTypes.PlayerSpawn)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), spawnAreaPlayer, false, WallTypes.PlayerSpawn);
                }
                else if (consturctedMap.wallMap[j + i * width] == WallTypes.EnemySpawn)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), spawnAreaEnemy, false, WallTypes.EnemySpawn);
                }
                else if (consturctedMap.wallMap[j + i * width] == WallTypes.Eagle)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), eagle, false, WallTypes.Eagle);
                }
            }
        }
    }

    public void PutObjectToCell(Vector3Int position, GameObject cube, bool is4Piece = false, WallTypes wallTypes = WallTypes.Empty)
    {
        Vector3 cellScale = grid.cellSize;
        Vector3Int realPositionOnGrid = position * new Vector3Int(2, 1, 2);

        if (is4Piece)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector3Int gridCoords = realPositionOnGrid + new Vector3Int(i, 0, j);
                    Vector3 worldPosition = grid.GetCellCenterWorld(gridCoords);
                    GameObject piece = Instantiate(cube, worldPosition, Quaternion.identity, gameObject.transform);
                    piece.transform.localScale = new Vector3(cellScale.x, cellScale.y, cellScale.z);
                }
            }
        }
        else
        {
            Vector3 worldPosition = Vector3.Lerp(grid.GetCellCenterWorld(realPositionOnGrid), grid.GetCellCenterWorld(realPositionOnGrid + new Vector3Int(1, 0, 1)), 0.5f);
            Instantiate(cube, worldPosition, Quaternion.identity, gameObject.transform);
        }
    }

    public void TryPutObjectToSelectedCellPosition(Vector3Int position, WallTypes wallType, bool is4Piece = false)
    {
        if (wallType == WallTypes.Empty)
        {
            RemoveObjectFromCell(position, is4Piece);
        }
        if (IsIndexEmpty(position))
        {
            SetIndex(position.x, position.z, width, wallType);
        }
        else
        {
            Debug.Log("Index not empty");
            return;
        }
        Vector3 cellScale = grid.cellSize;
        Vector3Int realPositionOnGrid = position * new Vector3Int(2, 1, 2);

        if (is4Piece)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector3Int gridCoords = realPositionOnGrid + new Vector3Int(i, 0, j);
                    Vector3 worldPosition = grid.GetCellCenterWorld(gridCoords);
                    GameObject piece = Instantiate(GetWall(wallType), worldPosition, Quaternion.identity, gameObject.transform);
                    piece.transform.localScale = new Vector3(cellScale.x, cellScale.y, cellScale.z);
                   
                }
            }
        }
        else
        {
            Vector3 worldPosition = Vector3.Lerp(grid.GetCellCenterWorld(realPositionOnGrid), grid.GetCellCenterWorld(realPositionOnGrid + new Vector3Int(1, 0, 1)), 0.5f);
            Instantiate(GetWall(wallType), worldPosition, Quaternion.identity, gameObject.transform);
        }
    }

    private int GetIndex(int x, int y, int width)
    {
        return y * width + x;
    }
    private void SetIndex(int x, int y, int width, WallTypes wallType)
    {
        consturctedMap.wallMap[GetIndex(x, y, width)] = wallType;
    }
    private bool IsIndexEmpty(Vector3Int position)
    {
       if (consturctedMap.wallMap[GetIndex(position.x, position.z, width)] == WallTypes.Empty)
       {
            return true;
       }

        return false;
    }

    public GameObject GetWall(WallTypes wallType)
    {
        switch (wallType)
        {
            default:
            case WallTypes.Bricks:
                return brickWall;
            case WallTypes.Static:
                return staticWall;
            case WallTypes.Grass:
                return grassWall;
            case WallTypes.PlayerSpawn:
                return spawnAreaPlayer;
            case WallTypes.EnemySpawn:
                return spawnAreaEnemy;
            case WallTypes.Eagle:
                return eagle;
        }
    }

    public bool CheckIsMapCorret()
    {
        int eagleCounter = 0;
        int playerSpawnCounter = 0;
        int enemySpawnCounter = 0;

        for (int i = 0; i < consturctedMap.wallMap.Length; i++)
        {
            if (consturctedMap.wallMap[i] == WallTypes.Eagle)
            {
                eagleCounter++;
            }
            else if (consturctedMap.wallMap[i] == WallTypes.PlayerSpawn)
            {
                playerSpawnCounter++;
            }
            else if (consturctedMap.wallMap[i] == WallTypes.EnemySpawn)
            {
                enemySpawnCounter++;
            }
        }

        if (eagleCounter == 1 && playerSpawnCounter == 1 && enemySpawnCounter > 0 && enemySpawnCounter <= 3)
        {
            // To do: Visual Feedback
            SaveMap();
            return true;
        }
        else
        {
            // To do: Visual Feedback
            return false;
        }
    }

    public void SaveMap()
    {
        Map savedMap = new Map();
        savedMap.wallMap = consturctedMap.wallMap;
        LevelSettings levelCopy = MapHandler.Instance.MapSettings.CopyData();
        GameManager.Instance.gameData.constructedLevelDataDic.Add(levelCopy.levelID, levelCopy);
    }
    public void RemoveObjectFromCell(Vector3Int position, bool is4Piece = false)
    {
        Debug.Log("REMOVE YAZMADIN MORUK");
        // Remove object from cell position
    }
}
