using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Raskulls.ScriptableSystem;
public class UI_Panel_Loading : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private Image sliderImage = null;

    public void OnToggleLoadingScreen(System.Boolean value)
    {
        canvasGroup.alpha = value ? 1 : 0;
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }
    public void OnLoadingProgress(System.Single value)
    {
        if (value == 0) sliderImage.fillAmount = 0.0001f;
        else sliderImage.fillAmount = value;
    }
}