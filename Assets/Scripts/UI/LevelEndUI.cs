using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEndUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelSituationText;

    [SerializeField] private Transform nextLevelButton;
    [SerializeField] private Transform restartLevelButton;

    public void SetLevelEndUI(bool isSuccess)
    {
        if (isSuccess)
        {
            levelSituationText.text = "LEVEL " + GameManager.Instance.CurrentLevelManager.CurrentLevel.level + " SUCCESSFULL";
            nextLevelButton.gameObject.SetActive(true);
            restartLevelButton.gameObject.SetActive(true);
        }
        else
        {
            levelSituationText.text = "LEVEL " + GameManager.Instance.CurrentLevelManager.CurrentLevel.level + " FAILED";
            restartLevelButton.gameObject.SetActive(true);
        }
    }
}
