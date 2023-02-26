using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(UI_BaseSubPanel),true)]
public class UI_BaseSubPanelEditor : Editor
{
    bool checkedShowing = false;
    string buttonText = "Show";
    bool showing = false;

    private void SetToggle(bool toggle)
    {
        showing = toggle;
        buttonText = showing ? "Hide" : "Show";
    }

    private void ToggleShowing()
    {
        showing = !showing;
        buttonText = showing ? "Hide" : "Show";
    }

    public override void OnInspectorGUI()
    {
        if (!checkedShowing)
        {
            SetToggle(((UI_BaseSubPanel)target).gameObject.GetComponentInChildren<CanvasGroup>(true).alpha == 1);
            checkedShowing = true;
        }

        if (GUILayout.Button(buttonText))
        {
            CanvasGroup myCanvasGroup = ((UI_BaseSubPanel)target).gameObject.GetComponentInChildren<CanvasGroup>();
            myCanvasGroup.alpha = (!showing) ? 1 : 0;
            myCanvasGroup.interactable = !showing;
            myCanvasGroup.blocksRaycasts = !showing;

            ToggleShowing();

            EditorUtility.SetDirty(myCanvasGroup);
            //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        base.OnInspectorGUI();
    }
}
