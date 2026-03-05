using TMPro;
using UnityEngine;

/// <summary>
/// Handles transitioning the game from menu to game scene and vice versa.
/// </summary>
public class LoadingPanel : MonoBehaviour
{
    /// <summary>
    /// The animator controlling the Loading panel.
    /// </summary>
    [SerializeField] private Animator animator;

    /// <summary>
    /// Whether this loading panel is in a gameplay scene.
    /// </summary>
    [SerializeField] private bool isInGameplayScene;

    /// <summary>
    /// The text object to display the game's name when transitioning to a game.
    /// </summary>
    [SerializeField] private TextMeshProUGUI gameNameText;

    /// <summary>
    /// Whether the scene to load is a game scene.
    /// </summary>
    private bool goingToGameScene;

    /// <summary>
    /// Determines what Enter animation needs to play.
    /// </summary>
    void Awake()
    {
        if (isInGameplayScene)
        {
            SetGameNameText();
            animator.SetTrigger("EnterGame");
        }
        else
        {
            animator.SetTrigger("EnterMenu");
        }
    }

    /// <summary>
    /// Plays the Going To Menu transition
    /// </summary>
    public void GoToMenuScene()
    {
        goingToGameScene = false;
        animator.SetTrigger("GoToMenu");
    }

    /// <summary>
    /// Plays the Going To Game transition
    /// </summary>
    public void GoToChosenGameScene()
    {
        goingToGameScene = true;
        SetGameNameText();
        animator.SetTrigger("GoToGame");
    }

    /// <summary>
    /// When the transition animation is finished, actually switch the scene.
    /// </summary>
    public void GoToAnimationFinished()
    {
        if (goingToGameScene)
        {
            GameCollectionManager.collection.LoadChosenGameScene();
        }
        else
        {
            GameCollectionManager.collection.LoadGameMenuScene();
        }
    }

    /// <summary>
    /// Sets the text object to display the chosen game's name.
    /// </summary>
    private void SetGameNameText()
    {
        if (GameCollectionManager.collection == null)
            return;

        gameNameText.text = GameCollectionManager.collection.GetGameName();
    }
}
