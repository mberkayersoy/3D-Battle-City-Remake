using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLevelListPanelUI : MonoBehaviour
{
    private GameManager gameManager;
    private void OnEnable()
    {
        gameManager = GameManager.Instance;

        int counter = 1;
        foreach (LevelUIElement item in transform.GetComponentsInChildren<LevelUIElement>())
        {
            item.level = counter++;
            item.levelText.text = "Level " + item.level;

            if (item.level <= gameManager.gameData.activeMaxLevelID)
            {
                item.SetData(gameManager.gameData.defaultLevelDataDic[item.level].levelScore);
                item.button.interactable = true;
            }
            else
            {
                item.button.interactable = false;
            }

        }
    }
}
