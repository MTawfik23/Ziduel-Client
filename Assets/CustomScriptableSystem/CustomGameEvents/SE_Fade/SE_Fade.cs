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
    [CreateAssetMenu(fileName = "SE_Fade_Name", menuName = "Raskulls/Scriptable System/Events/Fade Event")]
    public class SE_Fade : GameEventBase
    {
        public bool fadeIn;
        public float duration;
        public Color color;
        private readonly List<SE_FadeListener> eventListeners = new List<SE_FadeListener>();
        public void Raise(bool fadeIn, float duration, Color color, OnEventComplete onEventComplete = null)
        {
            this.fadeIn = fadeIn;
            this.duration = duration;
            this.color = color;
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(fadeIn, duration, color, onEventComplete));
        }
        private IEnumerator RaiseEvent(bool fadeIn, float duration, Color color, OnEventComplete onEventComplete = null)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPreEventRaised(fadeIn, duration, color);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(fadeIn, duration, color);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPostEventRaised(fadeIn, duration, color);
            yield return null;
            if (onEventComplete != null) onEventComplete();
        }
        public override void RegisterListener(GameEventListenerBase listener)
        {
            if (!eventListeners.Contains((SE_FadeListener)listener))
                eventListeners.Add((SE_FadeListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase listener)
        {
            if (eventListeners.Contains((SE_FadeListener)listener))
                eventListeners.Remove((SE_FadeListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<bool, float, Color>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<bool, float, Color>)unityEvent, (UnityAction<bool, float, Color>)myMethod);
#endif
        }
    }
}
