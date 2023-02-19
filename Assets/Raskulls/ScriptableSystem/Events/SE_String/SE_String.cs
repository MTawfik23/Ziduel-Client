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
    [CreateAssetMenu(fileName = "SE_String_Name", menuName = "Raskulls/Scriptable System/Events/String Event")]
    public class SE_String : GameEventBase
    {
        public string Value;
        private readonly List<SE_StringListener> eventListeners = new List<SE_StringListener>();
        public void Raise(string Value, OnEventComplete onEventComplete = null)
        {
            this.Value = Value;
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(Value, onEventComplete));
        }
        public void Raise(string Value)
        {
            this.Value = Value;
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(Value));
        }
        private IEnumerator RaiseEvent(string Value, OnEventComplete onEventComplete = null)
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
            if (!eventListeners.Contains((SE_StringListener)listener))
                eventListeners.Add((SE_StringListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase listener)
        {
            if (eventListeners.Contains((SE_StringListener)listener))
                eventListeners.Remove((SE_StringListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<string>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<string>)unityEvent, (UnityAction<string>)myMethod);
#endif
        }
    }
}
