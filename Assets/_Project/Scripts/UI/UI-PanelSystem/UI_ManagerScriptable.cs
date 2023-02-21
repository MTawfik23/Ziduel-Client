using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Raskulls.ScriptableSystem;
using System;

[CreateAssetMenu(fileName = "UI Manager Scriptable", menuName = "Domino/UI/UI Manager", order = 1)]
public class UI_ManagerScriptable : ScriptableObject
{
    [Header("Panel")]
    [SerializeField] private SE_UIPanels UIPanelChanged = null;
    [SerializeField] private UIPanels currentPanel = UIPanels.None, previousPanel = UIPanels.None;

    [Space]
    [Header("Sub Panel")]
    [SerializeField] private SE_UISubPanels UISubPanelChanged = null;
    [SerializeField] private UISubPanels currentSubPanel = UISubPanels.None, previousSubPanel = UISubPanels.None;


    public UIPanels CurrentPanel { get => currentPanel; }
    public UIPanels PreviousPanel { get => previousPanel; }

    public UISubPanels CurrentSubPanel { get => currentSubPanel; }
    public UISubPanels PreviousSubPanel { get => previousSubPanel; }

    private void OnEnable()
    {
        currentPanel = UIPanels.None;
        previousPanel = UIPanels.None;

        currentSubPanel = UISubPanels.None;
        previousSubPanel = UISubPanels.None;
    }

    public void ChangePanel(UIPanels panel)
    {
        if (currentSubPanel != UISubPanels.None)
        {
            previousSubPanel = currentSubPanel;
            currentSubPanel = UISubPanels.None;
            //uiSubPanelChanged.Raise(currentSubPanel);
        }

        if (panel == currentPanel && currentPanel != UIPanels.None)
        {
            Debug.LogError("Panel Already Set : " + panel.ToString());
            return;
        }

        previousPanel = currentPanel;
        currentPanel = panel;

        UIPanelChanged.Raise(currentPanel);
    }

    public void ChangeSubPanel(UISubPanels subPanel)
    {
        if (subPanel == currentSubPanel && currentSubPanel != UISubPanels.None)
        {
            Debug.LogError("Panel Already Set : " + subPanel.ToString());
            return;
        }
        previousSubPanel = currentSubPanel;
        currentSubPanel = subPanel;

        UISubPanelChanged.Raise(currentSubPanel);
    }
}
