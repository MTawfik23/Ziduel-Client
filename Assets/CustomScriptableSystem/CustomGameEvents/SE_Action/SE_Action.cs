using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine.Events;
using System.Reflection;
using System.Collections;

using System;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "SE_Action_Name", menuName = "Raskulls/Scriptable System/Events/Action Event")]
    public class SE_Action : GameEventBase
    {
        public Action Value;
        private readonly List<SE_ActionListener> eventListeners = new List<SE_ActionListener>();
        public void Raise(Action Value, OnEventComplete onEventComplete = null)
        {
            this.Value = Value;
            if(GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(Value, onEventComplete));
        }
        private IEnumerator RaiseEvent(Action Value, OnEventComplete onEventComplete = null)
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
            if (!eventListeners.Contains((SE_ActionListener)listener))
                eventListeners.Add((SE_ActionListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase  listener)
        {
            if (eventListeners.Contains((SE_ActionListener)listener))
                eventListeners.Remove((SE_ActionListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<Action>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<Action>)unityEvent, (UnityAction<Action>)myMethod);
#endif
        }
    }
}
