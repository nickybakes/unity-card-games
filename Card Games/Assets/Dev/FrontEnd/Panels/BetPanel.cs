using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class BetPanel : MonoBehaviour
{
    [SerializeField] private GameViewManager viewManager;

    [SerializeField] private Animator animator;

    [SerializeField] private TextMeshProUGUI betNumberText;
    [SerializeField] private TextMeshProUGUI totalBalanceText;

    [SerializeField] private BetterButton increaseBetButton;
    [SerializeField] private BetterButton decreaseBetButton;
    [SerializeField] private BetterButton placeBetButton;

    private bool maximized;

    private CultureInfo cultureUS;

    public void Awake()
    {
        cultureUS = CultureInfo.CreateSpecificCulture("en-US");
    }

    public void OnIncreaseBetSubmitted()
    {
        if (!maximized)
            return;

        UserManager.user.TryIncreaseBet();
        UpdatePanelElements();
    }

    public void OnDecreaseBetSubmitted()
    {
        if (!maximized)
            return;

        UserManager.user.TryDecreaseBet();
        UpdatePanelElements();
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

        betNumberText.text = ParseAsDollarAmount(UserManager.user.CurrentBet);
        totalBalanceText.text = ParseAsDollarAmount(UserManager.user.Balance);
        increaseBetButton.Interactable = !UserManager.user.IsSelectedBetIndexAtMax();
        decreaseBetButton.Interactable = !UserManager.user.IsSelectedBetIndexAtMin();
        placeBetButton.Interactable = UserManager.user.IsBalanceHighEnoughToBet();
    }

    private string ParseAsDollarAmount(float amount)
    {
        return amount.ToString("C", cultureUS);
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
