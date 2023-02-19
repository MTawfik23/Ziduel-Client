using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_LoadSceneListener : GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_LoadScene Event;
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
        public void OnPreEventRaised(SV_GameScene sceneToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            PreResponse.Invoke(sceneToLoad, showLoadingScreen, fadeScreen);
        }
        public void OnEventRaised(SV_GameScene sceneToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            Response.Invoke(sceneToLoad, showLoadingScreen, fadeScreen);
        }
        public void OnPostEventRaised(SV_GameScene sceneToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            PostResponse.Invoke(sceneToLoad, showLoadingScreen, fadeScreen);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<SV_GameScene, bool, bool> { }
    }
}
