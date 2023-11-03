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

            if (item.level <= gameManager.gameData.availableLevelCount)
            {
                item.SetData(true, gameManager.gameData.levelDataDic[item.level].levelScore);
                item.button.interactable = true;
            }
            else
            {
                item.button.interactable = false;
            }

        }
    }

}
