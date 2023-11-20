using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructedMapListMenuUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private MapUIElement mapUIElementPrefab;
    [SerializeField] private MapChoicePanel choicePanel;

    private void Start()
    {
        choicePanel.OnMapDeletedAction += ChoicePanel_OnMapDeletedAction;
    }

    private void ChoicePanel_OnMapDeletedAction()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        GameManager gameManager = GameManager.Instance;
        foreach (MapUIElement item in content.GetComponentsInChildren<MapUIElement>())
        {
            Destroy(item.gameObject);
        }

        foreach (var item in gameManager.gameData.constructedLevelDataDic)
        {
            MapUIElement element = Instantiate(mapUIElementPrefab, content);
            element.OnMapSelectedAction += Element_OnMapSelectedAction;
            element.SetMapData(item.Key);
        }
    }

    private void Element_OnMapSelectedAction(int mapID)
    {
        SetActiveChoicePanel(true);
        choicePanel.mapID = mapID;
    }

    public void SetActiveChoicePanel(bool shouldActive)
    {
        choicePanel.gameObject.SetActive(shouldActive);
    }
}
