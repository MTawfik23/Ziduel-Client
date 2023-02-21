using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_BasePanel : MonoBehaviour
{
    [SerializeField] protected UI_ManagerScriptable uiManagerScriptable = null;
    protected UIPanels myPanel = UIPanels.None;
    protected CanvasGroup canvasGroup = null;
    private float fadeInTime = 0.5f, fadeOutTime = 0.1f;

    protected void OnEnable()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>(true);
    }

    public virtual void OnUIPanelChanged(UIPanels currentPanel)
    {
        var isActive = currentPanel == myPanel;

        if (currentPanel == UIPanels.None)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            PanelDeactivated();
            return;
        }

        DOTween.Pause(gameObject);
        canvasGroup.DOFade(isActive ? 1 : 0, isActive ? fadeInTime : fadeOutTime);
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;

        if (isActive) PanelActivated();
        else PanelDeactivated();

    }

    protected virtual void PanelActivated()
    {
        canvasGroup.gameObject.SetActive(true);
    }

    protected virtual void PanelDeactivated()
    {
        canvasGroup.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (uiManagerScriptable.CurrentPanel != myPanel) return;
        if (uiManagerScriptable.CurrentSubPanel != UISubPanels.None) return;
        if (!Input.GetKeyUp(KeyCode.Escape)) return;
        if (!canvasGroup.gameObject.activeInHierarchy) return;
        BackButtonClick();
    }

    protected virtual void BackButtonClick()
    {

    }
}
