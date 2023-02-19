using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_PlayerListener: GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_Player Event ;
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
        public void OnPreEventRaised(Player value)
        {
            PreResponse.Invoke(value);
        }
        public void OnEventRaised(Player value)
        {
            Response.Invoke(value);
        }
        public void OnPostEventRaised(Player value)
        {
            PostResponse.Invoke(value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<Player> { }
    }
}
