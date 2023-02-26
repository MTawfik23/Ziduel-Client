using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class UI_Panel_InGame : UI_BasePanel
{
    [SerializeField] private Button backButton = null;

    private void Awake()
    {
        myPanel = UIPanels.InGame;
        AssignButtonListeners();
    }

    private void AssignButtonListeners()
    {
        backButton.onClick.AddListener(BackButtonPressed);
    }

    protected override void PanelActivated()
    {
        base.PanelActivated();
    }

    protected override void PanelDeactivated()
    {
        base.PanelDeactivated();
    }

    private void BackButtonPressed()
    {
        Debug.Log("Back, request to end the match making request and go back home");
    }
}
