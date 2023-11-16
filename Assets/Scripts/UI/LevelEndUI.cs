using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEndUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelSituationText;
    [SerializeField] private TextMeshProUGUI levelScoreText;
    [SerializeField] private Transform nextLevelButton;
    [SerializeField] private Transform restartLevelButton;

    public void SetLevelEndUI(bool isSuccess, bool isDefaultLevel = true)
    {
        GameManager gameManager = GameManager.Instance;

        if (isDefaultLevel)
        {
            if (isSuccess)
            {
                levelSituationText.text = "LEVEL " + gameManager.CurrentLevelManager.CurrentLevel.levelID + " SUCCESSFUL";
                levelScoreText.text = "Total Score: " + gameManager.CurrentLevelManager.LevelScore;
                nextLevelButton.gameObject.SetActive(true);
            }
            else
            {
                levelSituationText.text = "LEVEL " + gameManager.CurrentLevelManager.CurrentLevel.levelID + " FAILED";
                levelScoreText.text = "Score: " + gameManager.CurrentLevelManager.LevelScore;
            }
            restartLevelButton.gameObject.SetActive(true);
        }
        else
        {
            restartLevelButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(false);
            if (isSuccess)
            {
                levelSituationText.text = "MY LEVEL " + gameManager.CurrentLevelManager.CurrentLevel.levelID + " SUCCESSFUL";
                levelScoreText.text = "Total Score: " + gameManager.CurrentLevelManager.LevelScore;
            }
            else
            {
                levelSituationText.text = "LEVEL " + gameManager.CurrentLevelManager.CurrentLevel.levelID + " FAILED";
                levelScoreText.text = "Score: " + gameManager.CurrentLevelManager.LevelScore;
            }
        }

    }

    private void OnDisable()
    {
        nextLevelButton.gameObject.SetActive(false);
    }
}
