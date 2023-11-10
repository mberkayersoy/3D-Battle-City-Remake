using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MapUIElement : MonoBehaviour
{
    [SerializeField] private int mapID;
    [SerializeField] private TextMeshProUGUI mapIDText;
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(MapSelected);
    }

    public void SetMapData(int mapID)
    {
        this.mapID = mapID;
        mapIDText.text = "My Level " + mapID;
    }

    private void MapSelected()
    {
        EventBus.PublishSelectConstructMap(mapID);
    }
}
