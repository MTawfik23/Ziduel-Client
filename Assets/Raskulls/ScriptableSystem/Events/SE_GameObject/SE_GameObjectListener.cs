using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_GameObjectListener : GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_GameObject Event;
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
        public void OnPreEventRaised(GameObject Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(GameObject Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(GameObject Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<GameObject> { }
    }
}
