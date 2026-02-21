using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex
{
    AppInit = 0,
    GameMenu = 1,
    BattleTest = 2,
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

    /// <summary>
    /// The current user's info.
    /// </summary>
    [SerializeField] private UserManager user;


    private SceneIndex currentScene = SceneIndex.AppInit;

    private Action currentCallback;

    private SceneIndex previousScene = SceneIndex.AppInit;

    #region Startup   
    private void Awake()
    {
        if (app != null && app != this)
        {
            Destroy(this);
        }
        else
        {
            app = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex((int)SceneIndex.AppInit))
            SwitchToScene(SceneIndex.GameMenu);
    }
    #endregion

    // Update is called once per frame
    void Update()
    {

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
