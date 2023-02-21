using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_BaseOverPanel : MonoBehaviour
{
    [SerializeField] protected UI_ManagerScriptable uiManagerScriptable = null;
    protected CanvasGroup canvasGroup = null;

    private float fadeInTime = 0.5f, fadeOutTime = 0.1f;
    private Tween fadeTween = null;

    private bool showing = false;

    public void ShowPanel(bool isActive)
    {
        if (!isActive && !showing)
        {
            return;
        }

        showing = isActive;

        DOTween.Pause(gameObject);

        if(fadeTween != null)
        {
            fadeTween.Kill();
        }

        fadeTween = canvasGroup.DOFade(isActive ? 1 : 0, isActive ? fadeInTime : fadeOutTime)
            .OnComplete(()=> {
                if (isActive) PanelActivated();
                else PanelDeactivated();
            });

        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }

    public virtual void PanelActivated()
    {
        canvasGroup.gameObject.SetActive(true);
    }

    public virtual void PanelDeactivated()
    {
        canvasGroup.gameObject.SetActive(false);
    }
}
