using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine.Events;
using System.Reflection;
using System.Collections;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "SE_LoadScene_Name", menuName = "Raskulls/Scriptable System/Events/LoadScene Event")]
    public class SE_LoadScene : GameEventBase
    {
        public SV_GameScene sceneToLoad;
        public bool showLoadingScreen;
        public bool fadeScreen;
        private readonly List<SE_LoadSceneListener> eventListeners = new List<SE_LoadSceneListener>();
        public void Raise(SV_GameScene sceneToLoad, bool showLoadingScreen, bool fadeScreen, OnEventComplete onEventComplete = null)
        {
            this.sceneToLoad = sceneToLoad;
            this.showLoadingScreen = showLoadingScreen;
            this.fadeScreen = fadeScreen;
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(sceneToLoad, showLoadingScreen, fadeScreen, onEventComplete));
        }
        private IEnumerator RaiseEvent(SV_GameScene sceneToLoad, bool showLoadingScreen, bool fadeScreen, OnEventComplete onEventComplete = null)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPreEventRaised(sceneToLoad, showLoadingScreen, fadeScreen);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(sceneToLoad, showLoadingScreen, fadeScreen);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPostEventRaised(sceneToLoad, showLoadingScreen, fadeScreen);
            yield return null;
            if (onEventComplete != null) onEventComplete();
        }
        public override void RegisterListener(GameEventListenerBase listener)
        {
            if (!eventListeners.Contains((SE_LoadSceneListener)listener))
                eventListeners.Add((SE_LoadSceneListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase listener)
        {
            if (eventListeners.Contains((SE_LoadSceneListener)listener))
                eventListeners.Remove((SE_LoadSceneListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<SV_GameScene, bool, bool>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<SV_GameScene, bool, bool>)unityEvent, (UnityAction<SV_GameScene, bool, bool>)myMethod);
#endif
        }
    }
}
