using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class UI_Panel_WaitingForMatch : UI_BasePanel
{
    [SerializeField] private TextMeshProUGUI funFactText = null;
    [SerializeField] private RectTransform loadingIndicator = null;
    [SerializeField] private Button backButton = null;


    DG.Tweening.Core.TweenerCore<Quaternion, Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> rotateTween;

    private void Awake()
    {
        myPanel = UIPanels.WaitingForMatch;
        AssignButtonListeners();
    }

    private void AssignButtonListeners()
    {
        backButton.onClick.AddListener(BackButtonPressed);
    }

    protected override void PanelActivated()
    {
        base.PanelActivated();
        funFactText.text = "Random <b>fun fact or news</b> here!";

        rotateTween.Kill();
        rotateTween = loadingIndicator.DOLocalRotate(new Vector3(0, 0, 350), 0.5f).SetEase(Ease.InBounce).SetLoops(-1,LoopType.Yoyo);
    }

    protected override void PanelDeactivated()
    {
        base.PanelDeactivated();
        rotateTween.Kill();
    }

    private void BackButtonPressed()
    {
        Debug.Log("Back, request to end the match making request and go back home");
    }
}
