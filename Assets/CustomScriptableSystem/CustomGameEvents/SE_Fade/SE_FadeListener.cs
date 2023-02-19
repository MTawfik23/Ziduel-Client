using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_FadeListener : GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_Fade Event;
        public UnityEventReborn PreResponse;
        public UnityEventReborn Response;
        public UnityEventReborn PostResponse;
        private void OnEnable()
        {
            Event.RegisterListener(this);
        }
        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }
        public void OnPreEventRaised(bool fadeIn, float duration, Color color)
        {
            PreResponse.Invoke(fadeIn, duration, color);
        }
        public void OnEventRaised(bool fadeIn, float duration, Color color)
        {
            Response.Invoke(fadeIn, duration, color);
        }
        public void OnPostEventRaised(bool fadeIn, float duration, Color color)
        {
            PostResponse.Invoke(fadeIn, duration, color);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<bool, float, Color> { }
    }
}
