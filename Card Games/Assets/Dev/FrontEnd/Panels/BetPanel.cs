using System;
using System.Globalization;
using TMPro;
using UnityEngine;

/// <summary>
/// The Panel that lets the user adjust their bet, view the paytable, and place the bet.
/// </summary>
public class BetPanel : MonoBehaviour
{
    /// <summary>
    /// Reference to the Game View Manager.
    /// </summary>
    [SerializeField] private GameViewManager viewManager;

    /// <summary>
    /// Reference to the animator controlling the bet panel
    /// </summary>
    [SerializeField] private Animator animator;

    /// <summary>
    /// The Text Display that shows the currently selected bet value.
    /// </summary>
    [SerializeField] private TextDisplay betNumberText;

    /// <summary>
    /// The Text Display that shows the User's current balance.
    /// </summary>
    [SerializeField] private TextDisplay totalBalanceText;

    /// <summary>
    /// The button to submit to increase the bet.
    /// </summary>
    [SerializeField] private BetterButton increaseBetButton;

    /// <summary>
    /// The button to submit to decrease the bet.
    /// </summary>
    [SerializeField] private BetterButton decreaseBetButton;

    /// <summary>
    /// The button to submit to place the bet.
    /// </summary>
    [SerializeField] private BetterButton placeBetButton;

    /// <summary>
    /// The win display that shows the user's current winnings and adds it to the balance.
    /// </summary>
    [SerializeField] private WinDisplay winDisplay;

    /// <summary>
    /// Whether this panel is maximized on screen or not.
    /// </summary>
    private bool maximized;

    /// <summary>
    /// Try to increase the user's bet.
    /// </summary>
    public void OnIncreaseBetSubmitted()
    {
        if (!maximized)
            return;

        if (UserManager.user.TryIncreaseBet())
        {
            viewManager.UpdatePaytable();
            UpdatePanelElements();
        }
    }

    /// <summary>
    /// Try to decrease the user's bet.
    /// </summary>
    public void OnDecreaseBetSubmitted()
    {
        if (!maximized)
            return;

        if (UserManager.user.TryDecreaseBet())
        {
            viewManager.UpdatePaytable();
            UpdatePanelElements();
        }
    }

    /// <summary>
    /// Try to place the user's bet, if the user has enough in their balance.
    /// </summary>
    public void OnPlaceBetSubmitted()
    {
        if (!maximized)
            return;

        if (UserManager.user.TryPlaceBet())
        {
            UpdatePanelElements();
            viewManager.BetPlaced();
        }
    }

    /// <summary>
    /// Updates shown bet and balance values, and updates what buttons are enabled.
    /// </summary>
    public void UpdatePanelElements()
    {
        if (UserManager.user == null)
            return;

        betNumberText.SetText(ParseAsCurrency(UserManager.user.CurrentBet));
        totalBalanceText.SetText(ParseAsCurrency(UserManager.user.Balance));
        increaseBetButton.Interactable = !UserManager.user.IsSelectedBetIndexAtMax();
        decreaseBetButton.Interactable = !UserManager.user.IsSelectedBetIndexAtMin();
        placeBetButton.Interactable = UserManager.user.IsBalanceHighEnoughToBet();
    }

    /// <summary>
    /// Play the Maximize animation.
    /// </summary>
    public void MaximizePanel()
    {
        maximized = true;
        UpdatePanelElements();
        animator.SetTrigger("Maximize");
    }

    /// <summary>
    /// Play the Minimize animation.
    /// </summary>
    public void MinimizePanel()
    {
        maximized = false;
        animator.SetTrigger("Minimize");
    }

    /// <summary>
    /// If the user has winnings, use the Win Display to show them.
    /// </summary>
    public void ShowUserWinnings()
    {
        if (UserManager.user.Winnings > 0)
            winDisplay.ShowWinnings(ParseAsCurrency(UserManager.user.Winnings), UpdatePanelElements, UserManager.user.Winnings > UserManager.user.CurrentBet);
    }

    /// <summary>
    /// Parse a value as a currency string.
    /// </summary>
    /// <param name="amount">The value amount.</param>
    /// <returns>The parsed currency string.</returns>
    private string ParseAsCurrency(float amount)
    {
        return amount.ToString("C", UserManager.user.Culture);
    }
}
