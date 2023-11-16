using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelListUI : MonoBehaviour
{
    [SerializeField] private GameObject DefaultLevelListPanel;
    [SerializeField] private GameObject MyLevelListPanel;
    [SerializeField] private Button ShowDefaultLevelsButton;
    [SerializeField] private Button ShowMyLevelsButton;

    private void OnEnable()
    {
        SetActivePanel(DefaultLevelListPanel.name);
        ShowDefaultLevelsButton.Select();
    }

    private void Start()
    {
        ShowDefaultLevelsButton.onClick.AddListener(() => SetActivePanel(DefaultLevelListPanel.name));
        ShowMyLevelsButton.onClick.AddListener(() => SetActivePanel(MyLevelListPanel.name));
    }
    public void SetActivePanel(string activePanel)
    {
        DefaultLevelListPanel.SetActive(activePanel.Equals(DefaultLevelListPanel.name));
        MyLevelListPanel.SetActive(activePanel.Equals(MyLevelListPanel.name));
    }

}
