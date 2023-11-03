using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TransitionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;
    public Image loadingImage;
    public float animationDuration = 2.0f;
    private string nextPanel;

    private void OnEnable()
    {
        AnimateLoading();
    }

    public void SetNextPanel(string nextPanel)
    {
        this.nextPanel = nextPanel;
    }
    public void SetInfo(string info)
    {
        infoText.text = info;
    }

    private void AnimateLoading()
    {
        loadingImage.fillAmount = 0f;

        loadingImage.DOFillAmount(1f, animationDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                EventBus.PublishTransitionFinish(this, nextPanel);
            });
    }
    private void OnDisable()
    {
        DOTween.KillAll();
    }
}





