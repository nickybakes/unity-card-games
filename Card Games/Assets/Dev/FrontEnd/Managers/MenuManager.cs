using UnityEngine;

/// <summary>
/// Handles the view of the Menu where the player can choose a game to play.
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the Loading Panel to allow for transitioning to a game.
    /// </summary>
    [SerializeField] private LoadingPanel loadingPanel;

    /// <summary>
    /// The text that displays the user's balance.
    /// </summary>
    [SerializeField] private TextDisplay totalBalanceText;

    /// <summary>
    /// The text that displays the currently selected game's description.
    /// </summary>
    [SerializeField] private TextDisplay gameDescriptionText;

    /// <summary>
    /// The text that displays when no game is being selected.
    /// </summary>
    [SerializeField] private TextDisplay placeholderDescriptionText;

    /// <summary>
    /// Reference to a full screen raycast target to block player interactions.
    /// </summary>
    [SerializeField] private GameObject uiRaycastShield;

    /// <summary>
    /// Whether a game has been chosen or not.
    /// Keeps a game's description showing after its chosen and during the load transition.
    /// </summary>
    private bool gameHasBeenChosen = false;

    /// <summary>
    /// On Start, reset elements to their proper state.
    /// </summary>
    void Start()
    {
        UpdateElements();
        gameDescriptionText.Hide();
    }

    /// <summary>
    /// Hides a game's description and shows the placeholder one.
    /// </summary>
    public void HideGameDescription()
    {
        if (!gameHasBeenChosen)
        {
            gameDescriptionText.Hide();
            placeholderDescriptionText.Show();
        }
    }

    /// <summary>
    /// Shows a game's description and hides the placeholder one.
    /// </summary>
    /// <param name="gameChoice">The choise of game to show the description of.</param>
    public void ShowGameDescription(GameChoice gameChoice)
    {
        placeholderDescriptionText.Hide();
        gameDescriptionText.SetText(GameCollectionManager.collection.GetGameDescription(gameChoice), true);
        gameDescriptionText.Show();
    }

    /// <summary>
    /// When the user has clicked on a game's button, begin transitioning to that game.
    /// </summary>
    /// <param name="gameChoice">The game the player chose.</param>
    public void PlayGameSubmitted(GameChoice gameChoice)
    {
        gameHasBeenChosen = true;
        GameCollectionManager.collection.CurrentlyChosenGame = gameChoice;
        loadingPanel.GoToChosenGameScene();
    }

    /// <summary>
    /// When the user has clicked the Quit button to close the application.
    /// </summary>
    public void QuitSubmitted()
    {
        AppManager.app.QuitApp();
    }

    /// <summary>
    /// Updates the on screen elements to reflect their current values.
    /// </summary>
    public void UpdateElements()
    {
        if (UserManager.user == null)
            return;

        totalBalanceText.SetText(ParseAsCurrency(UserManager.user.Balance));
    }

    /// <summary>
    /// Deposits an amount in the player's balance and updates the elements.
    /// </summary>
    /// <param name="amount">The amount to deposit.</param>
    public void DepositAmount(int amount)
    {
        UserManager.user.DepositAmount(amount);
        UpdateElements();
    }

    /// <summary>
    /// Parses a value to a currency string.
    /// </summary>
    /// <param name="amount">The amount of money.</param>
    /// <returns>The currency string.</returns>
    private string ParseAsCurrency(float amount)
    {
        return amount.ToString("C", UserManager.user.Culture);
    }
}
