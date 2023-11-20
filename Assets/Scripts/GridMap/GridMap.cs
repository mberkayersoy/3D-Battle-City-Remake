using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEngine;
public enum WallTypes
{
    Empty = 0,
    Bricks,
    Static,
    Grass,
    Border,
    PlayerSpawn,
    EnemySpawn,
    Eagle
}
public class GridMap : MonoBehaviour
{
    public WallTypes[] wallMap;
    public MapSO currentMapSO;
    public Map storedMap;
    private const int width = 15;
    private const int height = 15;
    Grid grid;
    public GameObject groundCube;
    public GameObject border;
    public GameObject brickWall;
    public GameObject staticWall;
    public GameObject grassWall;
    public GameObject eagle;
    public GameObject spawnAreaPlayer;
    public GameObject spawnAreaEnemy;
    public Transform mainTargetTransform;
    public Transform playerSpawnTransform;
    public List<Transform> enemySpawnTransform = new List<Transform>();
    public List<Vector3> emptyCellPositions = new List<Vector3>();

    private void Awake()
    {
        grid = GetComponent<Grid>();
 
    }
    void Start()
    {
        if (currentMapSO != null)
        {
            wallMap = currentMapSO.wallMap;
        }
        else
        {
            //wallMap = storedMap.wallMap;
        }
        ClearMap();
        ConstructGround(width, height);
        ConstructMap();
    }

    public void ClearMap()
    {
        // Destory all previous childs
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            if (Application.isEditor)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
            else
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void ConstructGround(int width, int height)
    {
        // Create ground as one piece
        Vector3 centerPosition = Vector3.Lerp(grid.GetCellCenterWorld(new Vector3Int(0, 0, 0)), grid.GetCellCenterWorld(new Vector3Int(width * 2 -1, 0, height * 2 -1)), 0.5f);
        GameObject cell = Instantiate(groundCube, centerPosition, Quaternion.identity, gameObject.transform);
        cell.transform.localScale = new Vector3(width, 1, height);
        cell.name = "Ground";
        cell.AddComponent<NavMeshManager>();
    }

    public void ConstructMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (wallMap[j + i * width] == WallTypes.Empty)
                {
                    AddEmptyCellPosition(i, j);
                    SetIndex(j, i, width, WallTypes.Empty);
                }
                else if (wallMap[j + i * width] == WallTypes.Bricks)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), brickWall, true, WallTypes.Bricks);
                    SetIndex(j, i, width, WallTypes.Bricks);
                }
                else if (wallMap[j + i * width] == WallTypes.Border)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), border, false, WallTypes.Border);
                    SetIndex(j, i, width, WallTypes.Border);
                }
                else if (wallMap[j + i * width] == WallTypes.Static)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), staticWall, false, WallTypes.Static);
                    SetIndex(j, i, width, WallTypes.Static);
                }
                else if (wallMap[j + i * width] == WallTypes.Grass)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), grassWall, false, WallTypes.Grass);
                    SetIndex(j, i, width, WallTypes.Grass);
                }
                else if (wallMap[j + i * width] == WallTypes.PlayerSpawn)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), spawnAreaPlayer, false, WallTypes.PlayerSpawn);
                    SetIndex(j, i, width, WallTypes.PlayerSpawn);
                }
                else if (wallMap[j + i * width] == WallTypes.EnemySpawn)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), spawnAreaEnemy, false, WallTypes.EnemySpawn);
                    SetIndex(j, i, width, WallTypes.EnemySpawn);
                }
                else if (wallMap[j + i * width] == WallTypes.Eagle)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), eagle, false, WallTypes.Eagle);
                    SetIndex(j, i, width, WallTypes.Eagle);
                }
            }
        }

        //for (int i = 0; i < currentMap.wallMap.Length; i++)
        //{
        //    Debug.Log("currentMap.wallMap[" + i + "]" + currentMap.wallMap[i]);
        //}
    }

    private void SetIndex(int x, int y, int width, WallTypes wallType)
    {
        wallMap[y * width + x] = wallType;
        //return y * width + x;
    }

    public void AddEmptyCellPosition(int i, int j)
    {
        if (i <= 2 || i >= width - 1 || j <= 2 || j >= width) return;

        Vector3Int gridCoords = new Vector3Int(i * 2, 0, j * 2);
        Vector3 worldPosition = grid.GetCellCenterWorld(gridCoords);
        emptyCellPositions.Add(worldPosition);
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
                    GameObject piece = ObjectPool.Instance.GetObject(wallTypes);
                    piece.SetActive(true);
                    piece.transform.position = worldPosition;
                    piece.transform.localScale = new Vector3(cellScale.x, cellScale.y, cellScale.z);
                    piece.name = "Cell: " + position.x + "-" + position.z + " Grid: (" + gridCoords.x + "," + gridCoords.z + ")";
                }
            }
        }
        else
        {
            Vector3 worldPosition = Vector3.Lerp(grid.GetCellCenterWorld(realPositionOnGrid), grid.GetCellCenterWorld(realPositionOnGrid + new Vector3Int(1, 0, 1)), 0.5f);
            GameObject cell = ObjectPool.Instance.GetObject(wallTypes);
            cell.SetActive(true);
            cell.transform.position = worldPosition;
            cell.name = "Cell: " + position.x + "-" + position.z;

            if (wallTypes == WallTypes.EnemySpawn)
            {
                enemySpawnTransform.Add(cell.transform.GetChild(0));
            }
            else if (wallTypes == WallTypes.PlayerSpawn)
            {
                playerSpawnTransform = cell.transform.GetChild(0);
            }
            else if (wallTypes == WallTypes.Eagle)
            {
                mainTargetTransform = cell.transform.GetChild(0);
            }
        }
    }
}
