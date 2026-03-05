using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Forces the game to load the App Init scene. Useful in the editor if the game is launched from the wrong scene, 
/// this will automatically switch to the right one.
/// </summary>
public class ForceAppInit : MonoBehaviour
{
    void Start()
    {
        if (AppManager.app == null)
        {
            SceneManager.LoadScene((int)SceneIndex.AppInit, LoadSceneMode.Single);
            return;
        }
    }
}
