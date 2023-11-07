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

    public void SetLevelEndUI(bool isSuccess)
    {
        GameManager gameManager = GameManager.Instance;
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
    }

    private void OnDisable()
    {
        nextLevelButton.gameObject.SetActive(false);
    }
}
