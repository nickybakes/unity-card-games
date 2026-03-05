using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Game Collection Manager handles storing and loading the various games the player can play.
/// </summary>
public class GameCollectionManager : MonoBehaviour
{

    /// <summary>
    /// Singleton reference of the current Game Collection Manager.
    /// </summary>
    public static GameCollectionManager collection;

    /// <summary>
    /// List of GameRules for each game that can be played.
    /// </summary>
    [SerializeField] private List<GameRulesBase> gameCollection;

    /// <summary>
    /// The index of the Menu scene for selecting a game from.
    /// </summary>
    [SerializeField] private SceneIndex menuSceneIndex;

    /// <summary>
    /// The game the player has chosen to play.
    /// </summary>
    public GameChoice CurrentlyChosenGame { get; set; }

    /// <summary>
    /// Awake sets up the singleton and makes sure there is only one.
    /// </summary>
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

    /// <summary>
    /// Gets the description of the currently chosen game or a specific game.
    /// </summary>
    /// <param name="gameChoice">The game choice to read from. If left blank, uses the currently chosen game.</param>
    /// <returns>The description of the game.</returns>
    public string GetGameDescription(GameChoice gameChoice = null)
    {
        if (gameChoice == null)
            return gameCollection[(int)CurrentlyChosenGame.GameChoiceIndex].Description;

        return gameCollection[(int)gameChoice.GameChoiceIndex].Description;
    }

    /// <summary>
    /// Gets the name of the currently chosen game or a specific game.
    /// </summary>
    /// <param name="gameChoice">The game choice to read from. If left blank, uses the currently chosen game.</param>
    /// <returns>The name of the game.</returns>
    public string GetGameName(GameChoice gameChoice = null)
    {
        if (gameChoice == null)
            return gameCollection[(int)CurrentlyChosenGame.GameChoiceIndex].Name;

        return gameCollection[(int)gameChoice.GameChoiceIndex].Name;
    }

    /// <summary>
    /// Gets the scene index of the currently chosen game or a specific game.
    /// </summary>
    /// <param name="gameChoice">The game choice to read from. If left blank, uses the currently chosen game.</param>
    /// <returns>The scene index of the game.</returns>
    public SceneIndex GetGameSceneIndex(GameChoice gameChoice = null)
    {
        if (gameChoice == null)
            return gameCollection[(int)CurrentlyChosenGame.GameChoiceIndex].SceneToLoad;

        return gameCollection[(int)gameChoice.GameChoiceIndex].SceneToLoad;
    }

    /// <summary>
    /// Tells the AppManager to switch to the scene denoted in the currently chosen game.
    /// </summary>
    public void LoadChosenGameScene()
    {
        AppManager.app.SwitchToScene(GetGameSceneIndex(), InsertChosenGameRules);
    }

    /// <summary>
    /// Tells the AppManager to switch to the game menu scene.
    /// </summary>
    public void LoadGameMenuScene()
    {
        AppManager.app.SwitchToScene(menuSceneIndex);
    }

    /// <summary>
    /// Once a gameplay scene is loaded, the collection manager searched for a Game Manager to give the
    /// chosen game rules to in order to start playing the game.
    /// </summary>
    public void InsertChosenGameRules()
    {
        GameManagerBase gameManager = FindAnyObjectByType<GameManagerBase>();
        gameManager.LoadGameRules(gameCollection[(int)CurrentlyChosenGame.GameChoiceIndex]);
    }
}
