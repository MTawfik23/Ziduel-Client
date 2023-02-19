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
    [CreateAssetMenu(fileName = "SE_Player_Name", menuName = "Raskulls/Scriptable System/Events/Player Event")]
    public class SE_Player : GameEventBase
    {
        public Player Value;
        private readonly List<SE_PlayerListener> eventListeners = new List<SE_PlayerListener>();
        public void Raise(Player Value, OnEventComplete onEventComplete = null)
        {
            this.Value = Value;
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(Value,onEventComplete));
        }
        private IEnumerator RaiseEvent(Player value, OnEventComplete onEventComplete = null)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPreEventRaised(value);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(value);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPostEventRaised(value);
            yield return null;
            if (onEventComplete != null) onEventComplete();
        }
        public override void RegisterListener(GameEventListenerBase listener)
        {
            if (!eventListeners.Contains((SE_PlayerListener)listener))
                eventListeners.Add((SE_PlayerListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase  listener)
        {
            if (eventListeners.Contains((SE_PlayerListener)listener))
                eventListeners.Remove((SE_PlayerListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<Player>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<Player>)unityEvent, (UnityAction<Player>)myMethod);
#endif
        }
    }
}
