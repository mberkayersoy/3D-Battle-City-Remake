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

        int levelID = gameManager.CurrentLevelManager.CurrentLevel.levelID;
        int score = gameManager.CurrentLevelManager.LevelScore;
        if (isDefaultLevel)
        {
            if (isSuccess)
            {
                levelSituationText.text = "LEVEL " + levelID + " SUCCESSFUL";
                levelScoreText.text = "Total Score: " + score;
                nextLevelButton.gameObject.SetActive(true);
            }
            else
            {
                levelSituationText.text = "LEVEL " + levelID + " FAILED";
                levelScoreText.text = "Score: " + score;
            }
            restartLevelButton.gameObject.SetActive(true);
        }
        else
        {
            restartLevelButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(false);
            if (isSuccess)
            {
                levelSituationText.text = "MY LEVEL " + levelID + " SUCCESSFUL";
                levelScoreText.text = "Total Score: " + score;
            }
            else
            {
                levelSituationText.text = "LEVEL " + levelID + " FAILED";
                levelScoreText.text = "Score: " + score;
            }
        }

    }

    private void OnDisable()
    {
        nextLevelButton.gameObject.SetActive(false);
    }
}
