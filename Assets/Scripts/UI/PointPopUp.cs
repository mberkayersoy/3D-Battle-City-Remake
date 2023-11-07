using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PointPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentText;
    public float animationDuration = 1.0f;
    public Vector3 startScale = Vector3.zero;
    public Vector3 endScale = new Vector3(1.5f, 1.5f, 1.5f);

    private void Start()
    {
        //EventBus.OnScoreUpdateAction += EventBus_OnScoreUpdateAction;
        AnimateText();
    }

    public void SetContent(int score)
    {
        Debug.Log("SetContent = " + score);
        contentText.text = score.ToString();
    }
    //private void EventBus_OnScoreUpdateAction(object sender, EventBus.OnScoreUpdateEventArgs e)
    //{
    //    contentText.text = e.addScore.ToString();
    //    Debug.Log("e.addScore.ToString() = " + e.addScore);
    //}

    public void AnimateText()
    {
        contentText.transform.localScale = startScale;

        contentText.gameObject.SetActive(true);

        contentText.transform.DOScale(endScale, animationDuration).SetEase(Ease.OutBounce)
            .OnComplete(() => Destroy(gameObject));
    }
    private void OnDestroy()
    {
        //EventBus.OnScoreUpdateAction -= EventBus_OnScoreUpdateAction;
    }
}
