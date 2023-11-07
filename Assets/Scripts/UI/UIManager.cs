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
    [SerializeField] private Button quitGameButton;

    [Header("LEVEL LIST")]
    [SerializeField] private GameObject LevelListPanel;
    [SerializeField] private Button backToMenuButton;

    [Header("TRANSITION")]
    [SerializeField] private GameObject TransitionPanel;

    [Header("GAME")]
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private InformationView informationView;
    [SerializeField] private Button pauseButton;

    [Header("PAUSE")]
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button pauseRestartLevelButton;
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private Button continueButton;

    [Header("LEVEL END")]
    [SerializeField] private GameObject LevelEndPanel;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartLevelButton;
    [SerializeField] private Button menuButton;

    private GameManager gameManager;
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
        gameManager = GameManager.Instance;
        // Button listeners
        startGameButton.onClick.AddListener(OnClickStartGameButton);
        backToMenuButton.onClick.AddListener(OnClickBackToMenuButton);
        nextLevelButton.onClick.AddListener(OnClickNextLevelButton);
        restartLevelButton.onClick.AddListener(OnClickRestartLevelButton);
        pauseRestartLevelButton.onClick.AddListener(OnClickRestartLevelButton);
        pauseButton.onClick.AddListener(OnClickPauseButton);
        continueButton.onClick.AddListener(OnClickContinueButton);
        pauseMenuButton.onClick.AddListener(OnClickMenuButton);
        menuButton.onClick.AddListener(OnClickMenuButton);
        quitGameButton.onClick.AddListener(OnClickQuitGameButton);

        // Event listeners
        EventBus.OnLevelEndAction += EventBus_OnLevelEndAction;
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
        informationView.SetInformation(gameManager.CurrentLevelManager);
    }

    private void OnClickPauseButton()
    {
        SetActivePanel(PausePanel.name);
        Time.timeScale = 0;
    }

    private void OnClickContinueButton()
    {
        Time.timeScale = 1;
        SetActivePanel(GamePanel.name);
    }
    private void OnClickNextLevelButton()
    {
        Time.timeScale = 1;
        OnClickNextLevelAction?.Invoke();
        SetActivePanel(TransitionPanel.name);
        string info =  "Next level is loading...";
        TransitionPanel.GetComponent<TransitionUI>().SetNextPanel(GamePanel.name);
        TransitionPanel.GetComponent<TransitionUI>().SetInfo(info);
        informationView.SetInformation(gameManager.CurrentLevelManager);
    }
    
    private void OnClickBackToMenuButton()
    {
        SetActivePanel(MenuPanel.name);
    }

    private void OnClickQuitGameButton()
    {
        Application.Quit();
    }

    private void OnClickRestartLevelButton()
    {
        Time.timeScale = 1;
        OnClickRestartLevelAction?.Invoke();
        SetActivePanel(TransitionPanel.name);
        string info = "Level " + gameManager.gameData.currentLevelID.ToString() + " is loading again...";
        TransitionPanel.GetComponent<TransitionUI>().SetNextPanel(GamePanel.name);
        TransitionPanel.GetComponent<TransitionUI>().SetInfo(info);
        informationView.SetInformation(gameManager.CurrentLevelManager);
    }

    private void OnClickMenuButton()
    {
        Time.timeScale = 1;
        OnClickMenuAction?.Invoke();
        SetActivePanel(TransitionPanel.name);
        string info =  "Returning To Menu...";
        TransitionPanel.GetComponent<TransitionUI>().SetNextPanel(MenuPanel.name);
        TransitionPanel.GetComponent<TransitionUI>().SetInfo(info);
    }

    private void EventBus_OnLevelEndAction(object sender, EventBus.OnLevelEndEventArgs e)
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
        PausePanel.SetActive(activePanel.Equals(PausePanel.name));
        LevelEndPanel.SetActive(activePanel.Equals(LevelEndPanel.name));
    }
}
