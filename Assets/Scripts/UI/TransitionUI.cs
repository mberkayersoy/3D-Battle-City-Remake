using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TransitionUI : MonoBehaviour
{
    public Image loadingImage;
    public float animationDuration = 2.0f;
    public string nextPanel;

    private void OnEnable()
    {
        AnimateLoading();
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





