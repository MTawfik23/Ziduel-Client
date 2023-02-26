using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(UI_BasePanel), true)]
public class UI_BasePanelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("SetActivePanel"))
        {
            CanvasGroup myCanvasGroup = ((UI_BasePanel)target).gameObject.GetComponentInChildren<CanvasGroup>();
            var cgs = ((UI_BasePanel)target).gameObject.transform.root.GetComponentsInChildren<CanvasGroup>();
            foreach (var cg in cgs)
            {
                //if (cg.gameObject.GetComponent<UI_BasePanel>() == null) continue;
                cg.alpha = 0;
                cg.interactable = false;
                cg.blocksRaycasts = false;
                EditorUtility.SetDirty(cg);
            }
            myCanvasGroup.alpha = 1;
            myCanvasGroup.interactable = true;
            myCanvasGroup.blocksRaycasts = true;

            EditorUtility.SetDirty(myCanvasGroup);
            //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        base.OnInspectorGUI();
    }
}
