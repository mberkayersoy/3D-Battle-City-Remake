using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructedLevelListPanelUI : MonoBehaviour
{
    [SerializeField] private LevelUIElement constructedLevelUIElementPrefab;
    [SerializeField] private Transform contentTransform;
    private GameManager gameManager;
    
    private void OnEnable()
    {
        gameManager = GameManager.Instance;

        foreach (var item in contentTransform.GetComponentsInChildren<LevelUIElement>())
        {
            Destroy(item.gameObject);
        }

        foreach (var item in gameManager.gameData.constructedLevelDataDic)
        {
            LevelUIElement constructedLevelElement = Instantiate(constructedLevelUIElementPrefab, contentTransform);
            constructedLevelElement.level = item.Key;
            constructedLevelElement.levelText.text = "My Level " + item.Key.ToString();
            constructedLevelElement.SetData(item.Value.levelID);
        }
    }
}