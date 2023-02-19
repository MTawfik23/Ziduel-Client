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
    [CreateAssetMenu(fileName = "SE_Transform_Name", menuName = "Raskulls/Scriptable System/Events/Transform Event")]
    public class SE_Transform : GameEventBase
    {
        public Transform Value;
        private readonly List<SE_TransformListener> eventListeners = new List<SE_TransformListener>();
        public void Raise(Transform Value, OnEventComplete onEventComplete = null)
        {
            this.Value = Value;
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(Value, onEventComplete));
        }
        public void Raise(Transform Value)
        {
            this.Value = Value;
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(Value));
        }
        private IEnumerator RaiseEvent(Transform Value, OnEventComplete onEventComplete = null)
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
            if (!eventListeners.Contains((SE_TransformListener)listener))
                eventListeners.Add((SE_TransformListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase listener)
        {
            if (eventListeners.Contains((SE_TransformListener)listener))
                eventListeners.Remove((SE_TransformListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<Transform>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<Transform>)unityEvent, (UnityAction<Transform>)myMethod);
#endif
        }
    }
}
