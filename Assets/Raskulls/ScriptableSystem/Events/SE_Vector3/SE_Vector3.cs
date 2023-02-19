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
    [CreateAssetMenu(fileName = "SE_Vector3_Name", menuName = "Raskulls/Scriptable System/Events/Vector3 Event")]
    public class SE_Vector3 : GameEventBase
    {
        public Vector3 Value;
        private readonly List<SE_Vector3Listener> eventListeners = new List<SE_Vector3Listener>();
        public void Raise(Vector3 Value, OnEventComplete onEventComplete = null)
        {
            this.Value = Value;
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(Value,onEventComplete));
        }
        public void Raise(Vector3 Value)
        {
            this.Value = Value;
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(Value));
        }
        private IEnumerator RaiseEvent(Vector3 Value, OnEventComplete onEventComplete = null)
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
            if (!eventListeners.Contains((SE_Vector3Listener)listener))
                eventListeners.Add((SE_Vector3Listener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase  listener)
        {
            if (eventListeners.Contains((SE_Vector3Listener)listener))
                eventListeners.Remove((SE_Vector3Listener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<Vector3>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<Vector3>)unityEvent, (UnityAction<Vector3>)myMethod);
#endif
        }
    }
}
