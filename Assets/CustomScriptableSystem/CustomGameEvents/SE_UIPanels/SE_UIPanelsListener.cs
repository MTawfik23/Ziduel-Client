using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_UIPanelsListener: GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_UIPanels Event ;
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
        public void OnPreEventRaised(UIPanels Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(UIPanels Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(UIPanels Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<UIPanels> { }
    }
}
