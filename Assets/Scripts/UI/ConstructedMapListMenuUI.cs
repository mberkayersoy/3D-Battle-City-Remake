using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructedMapListMenuUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private MapUIElement mapUIElementPrefab;

    private void OnEnable()
    {
        GameManager gameManager = GameManager.Instance;
        foreach (MapUIElement item in content.GetComponentsInChildren<MapUIElement>())
        {
            Destroy(item.gameObject);
        }

        foreach (var item in gameManager.gameData.constructedLevelDataDic)
        {
            MapUIElement element = Instantiate(mapUIElementPrefab, content);
            element.SetMapData(item.Key);
        }
    }
}
