using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex
{
    AppInit = 0,
    GameMenu = 1,
    Poker = 2,
    BlackJack = 3,
}

/// <summary>
/// AppManager is the first object that loads in the game and handles overall app status including scene management.
/// </summary>
public class AppManager : MonoBehaviour
{

    /// <summary>
    /// The current App session.
    /// </summary>
    public static AppManager app;

    private SceneIndex currentScene = SceneIndex.AppInit;

    private Action currentCallback;

    private SceneIndex previousScene = SceneIndex.AppInit;

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

    // Update is called once per frame
    void Update()
    {

    }

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
    /// Switches to a scene asyncronously
    /// </summary>
    /// <param name="s">The scene you want to load</param>
    public void SwitchToScene(SceneIndex s, Action callback = null)
    {
        currentCallback = callback;
        SceneManager.sceneLoaded += WhenSceneDoneLoading;
        StartCoroutine(StartLoadProcess(s));

        previousScene = currentScene;
        currentScene = s;
    }

    private IEnumerator StartLoadProcess(SceneIndex s)
    {
        float time = 1f;
        if (currentScene == SceneIndex.AppInit)
            time = 0;
        yield return new WaitForSecondsRealtime(time);
        LoadScene(s);
    }

    private void LoadScene(SceneIndex s)
    {
        CheckIfLoadingDone(SceneManager.LoadSceneAsync((int)s, LoadSceneMode.Single));
    }

    private void WhenSceneDoneLoading(Scene scene, LoadSceneMode mode)
    {
        // UnloadScene(previousScene);
        if (currentCallback != null)
        {
            currentCallback.Invoke();
        }
        currentCallback = null;
        SceneManager.sceneLoaded -= WhenSceneDoneLoading;
    }

    public void UnloadScene(SceneIndex s)
    {
        SceneManager.UnloadSceneAsync((int)s);
    }

    private IEnumerator CheckIfLoadingDone(AsyncOperation operation)
    {
        if (!operation.isDone)
        {
            yield return null;
        }
    }

    public SceneIndex GetCurrentScene()
    {
        return currentScene;
    }

    #endregion
}
