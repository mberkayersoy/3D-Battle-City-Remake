using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Header("Game Objects")]
    public GameObject groundCube;
    public GameObject border;
    public GameObject brickWall;
    public GameObject staticWall;
    public GameObject grassWall;
    public GameObject eagle;
    public GameObject spawnAreaPlayer;
    public GameObject spawnAreaEnemy;
    public GameObject projectile;

    [Header("Construct Objects")]
    public GameObject constructBorder;
    public GameObject constructBrickWall;
    public GameObject constructStaticWall;
    public GameObject constructGrassWall;
    public GameObject constructEagle;
    public GameObject constructSpawnAreaPlayer;
    public GameObject constructSpawnAreaEnemy;

    [Header("Pool Sizes")]
    public int borderSize;
    public int enemySize;
    public int brickSize;
    public int statickSize;
    public int grassSize;
    public int playerSize;
    public int eagleSize;

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < borderSize; i++)
        {
            GameObject obj = Instantiate(border, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            borderList.Add(obj);
        }
        for (int i = 0; i < brickSize; i++)
        {
            GameObject obj = Instantiate(brickWall, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            brickList.Add(obj);
        }
        for (int i = 0; i < statickSize; i++)
        {
            GameObject obj = Instantiate(staticWall, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            statickList.Add(obj);
        }
        for (int i = 0; i < grassSize; i++)
        {
            GameObject obj = Instantiate(grassWall, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            grassList.Add(obj);
        }
        for (int i = 0; i < enemySize; i++)
        {
            GameObject obj = Instantiate(spawnAreaEnemy, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            enemyList.Add(obj);
        }
        for (int i = 0; i < playerSize; i++)
        {
            GameObject obj = Instantiate(spawnAreaPlayer, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            playerList.Add(obj);
        }
        for (int i = 0; i < eagleSize; i++)
        {
            GameObject obj = Instantiate(eagle, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            eagleList.Add(obj);
        }
    }

    public GameObject GetObject(WallTypes wallType)
    {
        switch (wallType)
        {
            default:
            case WallTypes.Bricks:
                foreach (GameObject pickup in brickList)
                {
                    if (!pickup.activeInHierarchy)
                    {
                        return pickup;
                    }
                }
                // If no inactive pick-up object is found, a new object is created and added to the pool.
                GameObject newbrick = Instantiate(brickWall, transform.position, Quaternion.identity);
                newbrick.SetActive(false);
                brickList.Add(newbrick);
                return newbrick;
            case WallTypes.Static:
                foreach (GameObject pickup in statickList)
                {
                    if (!pickup.activeInHierarchy)
                    {
                        return pickup;
                    }
                }
                GameObject newStatic = Instantiate(staticWall, transform.position, Quaternion.identity);
                newStatic.SetActive(false);
                statickList.Add(newStatic);
                return newStatic;
            case WallTypes.Grass:
                foreach (GameObject pickup in grassList)
                {
                    if (!pickup.activeInHierarchy)
                    {
                        return pickup;
                    }
                }
                GameObject newGrass = Instantiate(grassWall, transform.position, Quaternion.identity);
                newGrass.SetActive(false);
                grassList.Add(newGrass);
                return newGrass;
            case WallTypes.Border:
                foreach (GameObject pickup in borderList)
                {
                    if (!pickup.activeInHierarchy)
                    {
                        return pickup;
                    }
                }
                GameObject newBorder = Instantiate(border, transform.position, Quaternion.identity);
                newBorder.SetActive(false);
                borderList.Add(newBorder);
                return newBorder;
            case WallTypes.Eagle:
                foreach (GameObject pickup in eagleList)
                {
                    if (!pickup.activeInHierarchy)
                    {
                        return pickup;
                    }
                }
                GameObject newEagle = Instantiate(eagle, transform.position, Quaternion.identity);
                newEagle.SetActive(false);
                eagleList.Add(newEagle);
                return newEagle;
            case WallTypes.PlayerSpawn:
                foreach (GameObject pickup in playerList)
                {
                    if (!pickup.activeInHierarchy)
                    {
                        return pickup;
                    }
                }
                GameObject newPlayerSpawn = Instantiate(spawnAreaPlayer, transform.position, Quaternion.identity);
                newPlayerSpawn.SetActive(false);
                playerList.Add(newPlayerSpawn);
                return newPlayerSpawn;
            case WallTypes.EnemySpawn:
                foreach (GameObject pickup in enemyList)
                {
                    if (!pickup.activeInHierarchy)
                    {
                        return pickup;
                    }
                }
                GameObject newEnemySpawn = Instantiate(spawnAreaEnemy, transform.position, Quaternion.identity);
                newEnemySpawn.SetActive(false);
                enemyList.Add(newEnemySpawn);
                return newEnemySpawn;
        }
    }

    public void DeactivateAllObjects()
    {
        foreach (GameObject pickup in enemyList)
        {
            pickup.SetActive(false);
        }
        foreach (GameObject pickup in playerList)
        {
            pickup.SetActive(false);
        }
        foreach (GameObject pickup in brickList)
        {
            pickup.SetActive(false);
        }
        foreach (GameObject pickup in statickList)
        {
            pickup.SetActive(false);
        }
        foreach (GameObject pickup in borderList)
        {
            pickup.SetActive(false);
        }
        foreach (GameObject pickup in grassList)
        {
            pickup.SetActive(false);
        }
        foreach (GameObject pickup in eagleList)
        {
            pickup.SetActive(false);
        }
    }
}
