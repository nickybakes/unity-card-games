using System;
using System.Collections.Generic;
using UnityEngine;

public class GameCollectionManager : MonoBehaviour
{

    /// <summary>
    /// The current App session.
    /// </summary>
    public static GameCollectionManager collection;

    [SerializeField] private List<GameRulesBase> gameCollection;
    [SerializeField] private SceneIndex menuSceneIndex;
    public SceneIndex MenuSceneIndex { get => menuSceneIndex; }

    private GameChoice currentlyChosenGame;

    public GameChoice CurrentlyChosenGame { get => currentlyChosenGame; set => currentlyChosenGame = value; }

    private void Awake()
    {
        if (collection != null && collection != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            collection = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public string GetGameDescription(GameChoice gameChoice = null)
    {
        if (gameChoice == null)
            return gameCollection[(int)currentlyChosenGame.GameChoiceIndex].Description;

        return gameCollection[(int)gameChoice.GameChoiceIndex].Description;
    }

    public string GetGameName(GameChoice gameChoice = null)
    {
        if (gameChoice == null)
            return gameCollection[(int)currentlyChosenGame.GameChoiceIndex].Name;

        return gameCollection[(int)gameChoice.GameChoiceIndex].Name;
    }

    public SceneIndex GetGameSceneIndex(GameChoice gameChoice = null)
    {
        if (gameChoice == null)
            return gameCollection[(int)currentlyChosenGame.GameChoiceIndex].SceneToLoad;

        return gameCollection[(int)gameChoice.GameChoiceIndex].SceneToLoad;
    }

    public void LoadChosenGameScene()
    {
        AppManager.app.SwitchToScene(GetGameSceneIndex(), InsertChosenGameRules);
    }

    public void LoadGameMenuScene()
    {
        AppManager.app.SwitchToScene(MenuSceneIndex);
    }

    public void InsertChosenGameRules()
    {
        GameManagerBase gameManager = FindAnyObjectByType<GameManagerBase>();
        gameManager.LoadGameRules(gameCollection[(int)currentlyChosenGame.GameChoiceIndex]);
    }
}
