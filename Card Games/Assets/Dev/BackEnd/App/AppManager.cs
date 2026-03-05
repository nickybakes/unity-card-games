using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Index list of scenes in the build.
/// </summary>
public enum SceneIndex
{
    AppInit = 0,
    GameMenu = 1,
    Poker = 2,
    BlackJack = 3,
}

/// <summary>
/// AppManager is the first object that loads in the game and handles overall app status like scene management.
/// </summary>
public class AppManager : MonoBehaviour
{

    /// <summary>
    /// Singleton reference of the current App session.
    /// </summary>
    public static AppManager app;

    /// <summary>
    /// The index of the current scene that is loaded.
    /// </summary>
    private SceneIndex currentScene = SceneIndex.AppInit;

    /// <summary>
    /// The callback that will be called when a scene is finished loading.
    /// </summary>
    private Action currentCallback;

    /// <summary>
    /// The index of the previous scene that was loaded.
    /// </summary>
    private SceneIndex previousScene = SceneIndex.AppInit;

    /// <summary>
    /// Awake sets up the singleton and makes sure there is only one.
    /// </summary>
    private void Awake()
    {
        if (app != null && app != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            app = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Closes the entire App, whether in Editor or in build.
    /// </summary>
    public void QuitApp()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    #region Scene Switching
    /// <summary>
    /// Starts the process for loading a chosen scene and unloading the current one.
    /// </summary>
    /// <param name="sceneIndex">The index of the scene to be loaded.</param>
    /// <param name="callback">Optional callback function to call when the scene is finished loading.</param>
    public void SwitchToScene(SceneIndex sceneIndex, Action callback = null)
    {
        currentCallback = callback;
        SceneManager.sceneLoaded += WhenSceneDoneLoading;
        LoadScene(sceneIndex);

        previousScene = currentScene;
        currentScene = sceneIndex;
    }

    /// <summary>
    /// Tells Unity to load a chosen scene asynchronously.
    /// </summary>
    /// <param name="sceneIndex">The index of the scene to load.</param>
    private void LoadScene(SceneIndex sceneIndex)
    {
        SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Single);
    }

    /// <summary>
    /// Function that gets called when the scene is done loading. Calls the callback and resets it back to null.
    /// </summary>
    /// <param name="scene">The scene that Unity loaded.</param>
    /// <param name="mode">The Load Scene Mode for loading this scene.</param>
    private void WhenSceneDoneLoading(Scene scene, LoadSceneMode mode)
    {
        if (currentCallback != null)
        {
            currentCallback.Invoke();
        }
        currentCallback = null;
        SceneManager.sceneLoaded -= WhenSceneDoneLoading;
    }

    #endregion
}
