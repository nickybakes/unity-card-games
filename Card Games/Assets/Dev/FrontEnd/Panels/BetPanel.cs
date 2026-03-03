using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class BetPanel : MonoBehaviour
{
    [SerializeField] private GameViewManager viewManager;

    [SerializeField] private Animator animator;

    [SerializeField] private TextDisplay betNumberText;
    [SerializeField] private TextDisplay totalBalanceText;

    [SerializeField] private BetterButton increaseBetButton;
    [SerializeField] private BetterButton decreaseBetButton;
    [SerializeField] private BetterButton placeBetButton;

    [SerializeField] private WinDisplay winDisplay;

    private bool maximized;

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

    public void MaximizePanel()
    {
        maximized = true;
        UpdatePanelElements();
        animator.SetTrigger("Maximize");
    }

    public void MinimizePanel()
    {
        maximized = false;
        animator.SetTrigger("Minimize");
    }

    public void ShowUserWinnings()
    {
        if (UserManager.user.Winnings > 0)
            winDisplay.ShowWinnings(ParseAsCurrency(UserManager.user.Winnings), UpdatePanelElements, UserManager.user.Winnings > UserManager.user.CurrentBet);
    }

    private string ParseAsCurrency(float amount)
    {
        return amount.ToString("C", UserManager.user.Culture);
    }
}
