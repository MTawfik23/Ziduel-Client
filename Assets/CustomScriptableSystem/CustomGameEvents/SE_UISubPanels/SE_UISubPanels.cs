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
    [CreateAssetMenu(fileName = "SE_UISubPanels_Name", menuName = "Raskulls/Scriptable System/Events/UISubPanels Event")]
    public class SE_UISubPanels : GameEventBase
    {
        public UISubPanels Value;
        private readonly List<SE_UISubPanelsListener> eventListeners = new List<SE_UISubPanelsListener>();
        public void Raise(UISubPanels Value, OnEventComplete onEventComplete = null)
        {
            this.Value = Value;
            if(GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(Value, onEventComplete));
        }
        private IEnumerator RaiseEvent(UISubPanels Value, OnEventComplete onEventComplete = null)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPreEventRaised(Value);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(Value);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPostEventRaised(Value);
            yield return null;
            if (onEventComplete != null) onEventComplete();
        }
        public override void RegisterListener(GameEventListenerBase listener)
        {
            if (!eventListeners.Contains((SE_UISubPanelsListener)listener))
                eventListeners.Add((SE_UISubPanelsListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase  listener)
        {
            if (eventListeners.Contains((SE_UISubPanelsListener)listener))
                eventListeners.Remove((SE_UISubPanelsListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<UISubPanels>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<UISubPanels>)unityEvent, (UnityAction<UISubPanels>)myMethod);
#endif
        }
    }
}
