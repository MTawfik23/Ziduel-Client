using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_UISubPanelsListener: GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_UISubPanels Event ;
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
        public void OnPreEventRaised(UISubPanels Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(UISubPanels Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(UISubPanels Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<UISubPanels> { }
    }
}
