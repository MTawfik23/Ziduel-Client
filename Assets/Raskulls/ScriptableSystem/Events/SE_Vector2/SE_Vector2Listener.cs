using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_Vector2Listener: GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_Vector2 Event ;
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
        public void OnPreEventRaised(Vector2 Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(Vector2 Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(Vector2 Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<Vector2> { }
    }
}
