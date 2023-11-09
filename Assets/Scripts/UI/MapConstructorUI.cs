using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MapConstructorUI : MonoBehaviour
{
    [Header("WALL BUTTONS")]
    [SerializeField] private Button brickWallButton;
    [SerializeField] private Button grassWallButton;
    [SerializeField] private Button staticWallButton;
    [SerializeField] private Button eagleButton;
    [SerializeField] private Button enemySpawnButton;
    [SerializeField] private Button playerSpawnButton;
    [SerializeField] private Button eraserButton;
    [SerializeField] private Button saveMapButton;

    [Header("TEXTS")]
    [SerializeField] private TMP_InputField mapIDInpuField;
    [SerializeField] private TextMeshProUGUI playerLifeText;
    [SerializeField] private TextMeshProUGUI grayEnemyText;
    [SerializeField] private TextMeshProUGUI greenEnemyText;
    [SerializeField] private TextMeshProUGUI blueEnemyText;
    [SerializeField] private TextMeshProUGUI redEnemyText;

    [Header("SLIDERS")]
    [SerializeField] private Slider playerLifeSlider;
    [SerializeField] private Slider grayEnemySlider;
    [SerializeField] private Slider greenEnemySlider;
    [SerializeField] private Slider blueEnemySlider;
    [SerializeField] private Slider redEnemySlider;

    // EVENTS
    public event Action<int> OnPlayerLifeChangedAction;
    public event Action<int> OnLevelIDChangedAction;
    public event Action<int, EnemyType> OnEnemyCountChangedAction;
    public event Action<WallTypes> OnSelectedWallTypeChangedAction;
    public event Action OnSaveMapAction;

    private void Start()
    {
        playerLifeSlider.onValueChanged.AddListener(OnPlayerLifeChanged);
        mapIDInpuField.onValueChanged.AddListener(value => OnLevelIDChanged(value));
        grayEnemySlider.onValueChanged.AddListener(value => OnEnemyCountChanged(value, EnemyType.Gray));
        greenEnemySlider.onValueChanged.AddListener(value => OnEnemyCountChanged(value, EnemyType.Green));
        blueEnemySlider.onValueChanged.AddListener(value => OnEnemyCountChanged(value, EnemyType.Blue));
        redEnemySlider.onValueChanged.AddListener(value => OnEnemyCountChanged(value, EnemyType.Red));

        brickWallButton.onClick.AddListener(() => OnSelectedWallTypeChangedAction?.Invoke(WallTypes.Bricks));
        grassWallButton.onClick.AddListener(() => OnSelectedWallTypeChangedAction?.Invoke(WallTypes.Grass));
        staticWallButton.onClick.AddListener(() => OnSelectedWallTypeChangedAction?.Invoke(WallTypes.Static));
        eagleButton.onClick.AddListener(() => OnSelectedWallTypeChangedAction?.Invoke(WallTypes.Eagle));
        enemySpawnButton.onClick.AddListener(() => OnSelectedWallTypeChangedAction?.Invoke(WallTypes.EnemySpawn));
        playerSpawnButton.onClick.AddListener(() => OnSelectedWallTypeChangedAction?.Invoke(WallTypes.PlayerSpawn));
        eraserButton.onClick.AddListener(() => OnSelectedWallTypeChangedAction?.Invoke(WallTypes.Empty));
        saveMapButton.onClick.AddListener(() => OnSaveMapAction?.Invoke());
    }

    private void OnLevelIDChanged(string value)
    {
        if (value.Equals(""))
        {
            value = "0";
            mapIDInpuField.text = value;
        }
        OnLevelIDChangedAction?.Invoke(int.Parse(value));
    }

    private void OnPlayerLifeChanged(float value)
    {
        playerLifeText.text = "Player Life: " + value.ToString();
        OnPlayerLifeChangedAction?.Invoke((int)value);
    }

    private void OnEnemyCountChanged(float value, EnemyType enemyType)
    {
        SetText((int)value, enemyType);
        OnEnemyCountChangedAction?.Invoke((int)value, enemyType);
    }

    private void SetText(int value, EnemyType enemyType)
    {
        switch (enemyType)
        {
            default:
            case EnemyType.Gray:
                grayEnemyText.text = enemyType.ToString() + " Count: " + value.ToString();
                break;
            case EnemyType.Green:
                greenEnemyText.text = enemyType.ToString() + " Count: " + value.ToString();
                break;
            case EnemyType.Blue:
                blueEnemyText.text = enemyType.ToString() + " Count: " + value.ToString();
                break;
            case EnemyType.Red:
                redEnemyText.text = enemyType.ToString() + " Count: " + value.ToString();
                break;
        }
    }
}
