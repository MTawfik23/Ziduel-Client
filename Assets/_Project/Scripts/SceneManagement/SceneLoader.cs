using Raskulls.ScriptableSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [Header("Scriptable Variables")]
    [SerializeField] private SV_GameScene homeScreen = null;
    [Header("Scriptable Events")]
    [SerializeField] private SE_Bool toggleLoadingScreen = null;
    [SerializeField] private SE sceneReady = null;
    [SerializeField] private SE_Fade fadeRequest = null;
    [SerializeField] private SE_Float loadingProgress = null;
    //Parameters coming from scene loading requests
    private SV_GameScene sceneToLoad;
    private SV_GameScene currentlyLoadedScene;
    private bool showLoadingScreen;

    private float fadeDuration = .5f;
    private bool isLoading = false; //To prevent a new loading request while already loading a new scene


    /// <summary>
    /// This function loads the scene passed as parameter
    /// </summary>
    public void OnloadScene(SV_GameScene sceneToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        //Prevent a double-loading
        if (isLoading) return;
        this.sceneToLoad = sceneToLoad;
        this.showLoadingScreen = showLoadingScreen;
        isLoading = true;

        StartCoroutine(UnloadPreviousScene());
    }

    /// <summary>
    /// this function takes care of removing previously loaded scenes.
    /// </summary>
    private IEnumerator UnloadPreviousScene()
    {
        fadeRequest.Raise(false, fadeDuration, Color.black);

        yield return new WaitForSeconds(fadeDuration);

        if (currentlyLoadedScene != null) //would be null if the game was started in Initialisation
            yield return SceneManager.UnloadSceneAsync(currentlyLoadedScene.sceneReference);
        StartCoroutine(LoadNewScene());
    }

    /// <summary>
    /// Kicks off the asynchronous loading of a scene.
    /// </summary>
    private IEnumerator LoadNewScene()
    {
        if (showLoadingScreen) toggleLoadingScreen.Raise(true);
        
        if (sceneToLoad == null) yield break;
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad.sceneReference,LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;
        float progress = 0;
        loadingProgress.Raise(progress);
        while (progress < 0.9f || (asyncLoad.progress < 0.9f))
        {
            progress += 0.005f;
            loadingProgress.Raise(progress);
            yield return null;
        }
        
        asyncLoad.allowSceneActivation = true;
        currentlyLoadedScene = sceneToLoad;
        isLoading = false;
        if (showLoadingScreen) toggleLoadingScreen.Raise(false);
        fadeRequest.Raise(true, fadeDuration, Color.clear);
        StartGameplay();
    }
    private void StartGameplay()
    {
        sceneReady.Raise();
    }

    public void OnAuthSuccess()
    {
        OnloadScene(homeScreen, true, true);
    }
}