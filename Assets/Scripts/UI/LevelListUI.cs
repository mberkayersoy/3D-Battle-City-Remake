using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelListUI : MonoBehaviour
{
    private GameManager gameManager;
    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        int counter = 0;
        foreach (LevelUIElement item in transform.GetComponentsInChildren<LevelUIElement>())
        {
            item.level = counter++;
            item.levelText.text = "Level " + item.level;

            if (item.level <= gameManager.gameData.activeMaxLevelID)
            {
                //Debug.Log("ITEM LEVEL: " + item.level);
               // Debug.Log("gameManager.gameData.levelDataDic[item.level].score: " + gameManager.gameData.levelDataDic[item.level].levelScore);
                item.SetData(true, gameManager.gameData.defaultLevelDataDic[item.level].levelScore);
                item.button.interactable = true;
            }
            else
            {
                item.button.interactable = false;
            }

        }
    }

}
