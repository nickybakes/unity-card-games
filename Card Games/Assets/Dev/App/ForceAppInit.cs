using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Forces the game to load the App Init scene. Useful in the editor if you launch the game from the wrong scene, 
/// this will automatically put you in the right one.
/// </summary>
public class ForceAppInit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (AppManager.app == null)
        {
            SceneManager.LoadScene((int)SceneIndex.AppInit, LoadSceneMode.Single);
            return;
        }
    }
}
