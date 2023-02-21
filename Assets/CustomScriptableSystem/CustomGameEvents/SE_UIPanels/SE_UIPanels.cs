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
    [CreateAssetMenu(fileName = "SE_UIPanels_Name", menuName = "Raskulls/Scriptable System/Events/UIPanels Event")]
    public class SE_UIPanels : GameEventBase
    {
        public UIPanels Value;
        private readonly List<SE_UIPanelsListener> eventListeners = new List<SE_UIPanelsListener>();
        public void Raise(UIPanels Value, OnEventComplete onEventComplete = null)
        {
            this.Value = Value;
            if(GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(Value, onEventComplete));
        }
        private IEnumerator RaiseEvent(UIPanels Value, OnEventComplete onEventComplete = null)
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
            if (!eventListeners.Contains((SE_UIPanelsListener)listener))
                eventListeners.Add((SE_UIPanelsListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase  listener)
        {
            if (eventListeners.Contains((SE_UIPanelsListener)listener))
                eventListeners.Remove((SE_UIPanelsListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<UIPanels>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<UIPanels>)unityEvent, (UnityAction<UIPanels>)myMethod);
#endif
        }
    }
}
