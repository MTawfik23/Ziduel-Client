using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_BaseSubPanel : MonoBehaviour
{
    [SerializeField] protected UI_ManagerScriptable uiManagerScriptable = null;
    protected UISubPanels mySubPanel = UISubPanels.None;
    protected CanvasGroup canvasGroup = null;
    private float fadeInTime = 0.75f, fadeOutTime = 0.5f;

    protected void OnEnable()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>(true);
    }

    public virtual void OnUISubPanelChanged(UISubPanels currentSubPanel)
    {
        var isActive = currentSubPanel == mySubPanel;

        canvasGroup.DOFade(isActive ? 1 : 0, isActive ? fadeInTime : fadeOutTime);
        DOTween.Pause(canvasGroup.transform);
        canvasGroup.transform.DOScale(isActive ? 1 : 0, isActive ? fadeInTime : fadeOutTime)
            .SetEase((isActive) ? Ease.OutBack : Ease.InBack)
            .OnStart(() => { if (isActive) PanelActivated(); })
            .OnComplete(() => { if (!isActive) PanelDeactivated(); });

        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
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
        if (uiManagerScriptable.CurrentSubPanel != mySubPanel) return;
        if (!Input.GetKeyUp(KeyCode.Escape)) return;
        if (!canvasGroup.gameObject.activeInHierarchy) return;
        BackButtonClick();
    }

    protected virtual void BackButtonClick()
    {

    }
}
