using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapChoicePanel : MonoBehaviour
{
    public int mapID;
    [SerializeField] private Button updateButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button closeButton;

    public event Action OnMapDeletedAction;
    void Start()
    {
        updateButton.onClick.AddListener(OpenSelectedMap);
        deleteButton.onClick.AddListener(DeleteSelectedMap);
        closeButton.onClick.AddListener(ClosePanel);
    }

    public void OpenSelectedMap()
    {
        EventBus.PublishSelectConstructMap(mapID);
        gameObject.SetActive(false);
    }

    public void DeleteSelectedMap()
    {
        GameManager.Instance.gameData.constructedLevelDataDic.Remove(mapID);
        OnMapDeletedAction?.Invoke();
        gameObject.SetActive(false);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
