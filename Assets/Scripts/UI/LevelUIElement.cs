using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelUIElement : MonoBehaviour
{
    [SerializeField] public int level;
    [SerializeField] public TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public Button button;


    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LevelSelected);
    }
    public void SetData(int score = 0)
    {
        scoreText.text = score.ToString();
    }

    private void LevelSelected()
    {
        EventBus.PublishSelectedLevel(this, level);
    }
}
