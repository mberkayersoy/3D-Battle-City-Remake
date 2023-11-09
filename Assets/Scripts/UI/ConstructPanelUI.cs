using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject constructMenuPanel;
    [SerializeField] private GameObject constructMapPanel;

    [SerializeField] private Button constructorMapPanelButton;
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private Button backToConstructMenuPanelButton;

    public event Action OnClickBackToMenuButtonAction;
    private void OnEnable()
    {
        SetActivePanel(constructMenuPanel.name);
    }
    private void Start()
    {
        backToMainMenuButton.onClick.AddListener(OnClickBackToMenuButton);
        constructorMapPanelButton.onClick.AddListener(OnClickConstructorMapPanel);
        backToConstructMenuPanelButton.onClick.AddListener(OnClickBackToConstructMenuPanelButton);
    }

    private void OnClickBackToConstructMenuPanelButton()
    {
        SetActivePanel(constructMenuPanel.name);
    }

    private void OnClickConstructorMapPanel()
    {
        constructMapPanel.GetComponent<MapConstructor>().SetLevelSetting(null);
        SetActivePanel(constructMapPanel.name);
    }

    private void OnClickBackToMenuButton()
    {
        OnClickBackToMenuButtonAction?.Invoke();
    }

    public void SetActivePanel(string activePanel)
    {
        constructMenuPanel.SetActive(activePanel.Equals(constructMenuPanel.name));
        constructMapPanel.SetActive(activePanel.Equals(constructMapPanel.name));
    }
}
