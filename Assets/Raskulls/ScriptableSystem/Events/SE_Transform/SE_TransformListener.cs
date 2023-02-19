using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_TransformListener: GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_Transform Event ;
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
        public void OnPreEventRaised(Transform Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(Transform Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(Transform Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<Transform> { }
    }
}
