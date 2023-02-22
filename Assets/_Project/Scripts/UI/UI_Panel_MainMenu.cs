using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Raskulls.ScriptableSystem;

public class UI_Panel_MainMenu : UI_BasePanel
{
    [Header("Scriptables")]
    [SerializeField] private SE_LoadScene LoadScene = null;
    [SerializeField] private SV_GameScene onlineGameScene = null;
    [SerializeField] private SV_bool internetAvailable = null;

    [Space]
    [Header("UI")]
    //middle buttons
    [SerializeField] private Button playOnlineButton = null;


    [Space]
    //other UI Elements
    [SerializeField] private TextMeshProUGUI gameStateText = null;

    private void Awake()
    {
        myPanel = UIPanels.MainMenu;
        AssignButtonListeners();
    }

    private void Start()
    {
        OnInternetAvailabilityChanged(internetAvailable.Value);

        uiManagerScriptable.ChangePanel(UIPanels.MainMenu);
        uiManagerScriptable.ChangeSubPanel(UISubPanels.None);
    }

    protected override void PanelActivated()
    {
        base.PanelActivated();
        OnInternetAvailabilityChanged(internetAvailable.Value);
    }

    private void AssignButtonListeners()
    {
        playOnlineButton.onClick.AddListener(PlayOnlinePressed);
    }

    private void PlayOnlinePressed()
    {
        LoadScene.Raise(onlineGameScene, true, true);
    }


    public void OnInternetAvailabilityChanged(System.Boolean value)
    {
        //gameStateText.text = gameStateString + (value ?" <color=\"green\">Online</color>": " <color=\"red\">Offline</color>");
        string connectionStateText = value ? "Online" : "Offline";

        //gameStateText.text = gameStateString + (value ?$" <color=\"green\">{connectionStateText}</color>": $" <color=\"red\">{connectionStateText}</color>");
        gameStateText.text = (value ? $" <color=\"green\">{connectionStateText}</color>" : $" <color=\"red\">{connectionStateText}</color>");
    }
}
