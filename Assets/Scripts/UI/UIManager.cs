using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager _Instance;
    public static UIManager Instance
    {
        get
        {
            return _Instance;
        }

        private set
        {
            _Instance = value;
        }
    }

    public event Action OnStartSpawnAction;
    public event Action OnClickNextLevelAction;
    public event Action OnClickMenuAction;
    public event Action OnClickRestartLevelAction;

    [Header("MENU")]
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private Button startGameButton;

    [Header("LEVEL LIST")]
    [SerializeField] private GameObject LevelListPanel;

    [Header("TRANSITION")]
    [SerializeField] private GameObject TransitionPanel;

    [Header("GAME")]
    [SerializeField] private GameObject GamePanel;

    [Header("LEVEL END")]
    [SerializeField] private GameObject LevelEndPanel;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartLevelButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        startGameButton.onClick.AddListener(OnClickStartGameButton);
        nextLevelButton.onClick.AddListener(OnClickNextLevelButton);
        restartLevelButton.onClick.AddListener(OnClickRestartLevelButton);
        menuButton.onClick.AddListener(OnClickMenuButton);
        EventBus.OnLevelSuccessfullyEndAction += EventBus_OnLevelSuccessfullyEndAction;
        EventBus.OnLevelSelectedAction += EventBus_OnLevelSelectedAction;
        EventBus.OnTransitionFinishAction += EventBus_OnTransitionFinishAction;
        SetActivePanel(MenuPanel.name);
    }

    private void EventBus_OnTransitionFinishAction(object sender, EventBus.OnTransitionEventArgs e)
    {
        SetActivePanel(e.panel);
        if (!e.panel.Equals(MenuPanel.name))
        {
            OnStartSpawnAction?.Invoke();
        }
    }

    private void EventBus_OnLevelSelectedAction(object sender, EventBus.OnLevelSelectedEventArgs e)
    {
        SetActivePanel(TransitionPanel.name);
        string info = "Level " + e.selectedLevel.ToString() + " is loading...";
        TransitionPanel.GetComponent<TransitionUI>().SetNextPanel(GamePanel.name);
        TransitionPanel.GetComponent<TransitionUI>().SetInfo(info);
    }

    private void OnClickNextLevelButton()
    {
        OnClickNextLevelAction?.Invoke();
        SetActivePanel(TransitionPanel.name);
        string info =  "Next level is loading...";
        TransitionPanel.GetComponent<TransitionUI>().SetNextPanel(GamePanel.name);
        TransitionPanel.GetComponent<TransitionUI>().SetInfo(info);
    }

    private void OnClickRestartLevelButton()
    {
        OnClickRestartLevelAction?.Invoke();
        SetActivePanel(TransitionPanel.name);
        string info = "Level " + GameManager.Instance.gameData.currentLevel.ToString() + " is loading again...";
        TransitionPanel.GetComponent<TransitionUI>().SetNextPanel(GamePanel.name);
        TransitionPanel.GetComponent<TransitionUI>().SetInfo(info);
    }

    private void OnClickMenuButton()
    {
        OnClickMenuAction?.Invoke();
        SetActivePanel(TransitionPanel.name);
        string info =  "Returning To Menu...";
        TransitionPanel.GetComponent<TransitionUI>().SetNextPanel(MenuPanel.name);
        TransitionPanel.GetComponent<TransitionUI>().SetInfo(info);
    }

    private void EventBus_OnLevelSuccessfullyEndAction(object sender, EventBus.OnLevelSuccessfullyEndEventArgs e)
    {
        LevelEndPanel.GetComponent<LevelEndUI>().SetLevelEndUI(e.isSuccess);
        SetActivePanel(LevelEndPanel.name);
    }


    private void OnClickStartGameButton()
    {
        SetActivePanel(LevelListPanel.name);
    }

    public void SetActivePanel(string activePanel)
    {

        MenuPanel.SetActive(activePanel.Equals(MenuPanel.name));
        LevelListPanel.SetActive(activePanel.Equals(LevelListPanel.name));
        TransitionPanel.SetActive(activePanel.Equals(TransitionPanel.name));
        GamePanel.SetActive(activePanel.Equals(GamePanel.name));
        LevelEndPanel.SetActive(activePanel.Equals(LevelEndPanel.name));
    }

}
