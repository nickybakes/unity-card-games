using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/// <summary>
/// The User Manager handles setting, storing, and retrieving of the User's data such as market data, financial data, etc.
/// </summary>
public class UserManager : MonoBehaviour
{
    /// <summary>
    /// Singleton reference of the current User session.
    /// </summary>
    public static UserManager user;

    /// <summary>
    /// The info of the Market the User is in.
    /// </summary>
    [SerializeField] private MarketInfo marketInfo;

    /// <summary>
    /// The stored balance of the current User.
    /// </summary>
    private float balance;

    /// <summary>
    /// The index of the User's selected bet.
    /// </summary>
    private int currentSelectedBetIndex;

    /// <summary>
    /// The most recent amount of money won by the User.
    /// </summary>
    private float winnings;

    /// <summary>
    /// The Culture info of the User's Market. Useful for parsing numbers as currency strings.
    /// </summary>
    private CultureInfo culture;

    /// <summary>
    /// The index of the User's selected bet.
    /// </summary>
    public int CurrentSelectedBetIndex
    {
        get
        {
            return currentSelectedBetIndex;
        }
    }

    /// <summary>
    /// The User's selected bet.
    /// </summary>
    public float CurrentBet
    {
        get
        {
            return marketInfo.PossibleBets[currentSelectedBetIndex];
        }
    }

    /// <summary>
    /// The number of possible bets the User can pick from.
    /// </summary>
    public int NumberPossibleBets
    {
        get
        {
            return marketInfo.PossibleBets.Count;
        }
    }

    /// <summary>
    /// Getter for the balance of the current User.
    /// </summary>
    public float Balance
    {
        get
        {
            return balance;
        }
    }

    /// <summary>
    /// The most recent amount of money won by the User.
    /// </summary>
    public float Winnings
    {
        get
        {
            return winnings;
        }
    }

    /// <summary>
    /// The Culture info of the User's Market. Useful for parsing numbers as currency strings.
    /// </summary>
    public CultureInfo Culture
    {
        get
        {
            return culture;
        }
    }

    /// <summary>
    /// Awake sets up the singleton and makes sure there is only one.
    /// It also retrieves the Market Info of the User.
    /// </summary>
    void Awake()
    {
        if (user != null && user != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            user = this;
            DontDestroyOnLoad(gameObject);
        }

        GetMarketInfo();
    }

    /// <summary>
    /// Retrieves the market info data. If connecting to an online service,
    /// that would be done here to retrieve the data.
    /// </summary>
    private void GetMarketInfo()
    {
        balance = marketInfo.UserStartingBalance;
        currentSelectedBetIndex = marketInfo.DefaultBetIndex;
        culture = CultureInfo.CreateSpecificCulture(marketInfo.CultureCode);
    }

    /// <summary>
    /// Get the value of a bet at an index.
    /// </summary>
    /// <param name="index">The index of the bet.</param>
    /// <returns>The bet value.</returns>
    public float GetPossibleBet(int index)
    {
        return marketInfo.PossibleBets[index];
    }

    /// <summary>
    /// Adds winnings to the User's balance and stores the most recent winnings.
    /// </summary>
    /// <param name="_winnings">The winnings to add.</param>
    public void AwardWinnings(float _winnings)
    {
        winnings = _winnings;
        balance += winnings;
    }

    /// <summary>
    /// Adds an amount of money to the User's balance.
    /// </summary>
    /// <param name="amount">The amount to add.</param>
    public void DepositAmount(float amount)
    {
        balance += amount;
    }

    /// <summary>
    /// Tries to take the current bet amount from the balance. If the balance is too low, it does not take the bet amount.
    /// </summary>
    /// <returns>Returns true if there was enough balance to take the bet, false otherwise.</returns>
    public bool TryPlaceBet()
    {
        if (balance >= CurrentBet)
        {
            balance -= CurrentBet;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Tries to go to the next selectable bet. If the index could be increased, returns true.
    /// </summary>
    /// <returns>Returns true if selected bet index could be increased.</returns>
    public bool TryIncreaseBet()
    {
        if (IsSelectedBetIndexAtMax())
            return false;

        currentSelectedBetIndex = Math.Clamp(currentSelectedBetIndex + 1, 0, marketInfo.PossibleBets.Count - 1);
        return true;
    }

    /// <summary>
    /// Tries to go to the previous selectable bet. If the index could be decreased, returns true.
    /// </summary>
    /// <returns>Returns true if selected bet index could be decreased.</returns>
    public bool TryDecreaseBet()
    {
        if (IsSelectedBetIndexAtMin())
            return false;

        currentSelectedBetIndex = Math.Clamp(currentSelectedBetIndex - 1, 0, marketInfo.PossibleBets.Count - 1);
        return true;
    }

    /// <summary>
    /// Checks if a bet is too high for the current balance.
    /// </summary>
    /// <returns>True if the bet is too high, false otherwise.</returns>
    public bool IsBalanceHighEnoughToBet()
    {
        return balance >= CurrentBet;
    }

    /// <summary>
    /// Checks if the current selected bet index is the highest it can go.
    /// </summary>
    /// <returns>True if the selected bet index is at its maximum.</returns>
    public bool IsSelectedBetIndexAtMax()
    {
        return currentSelectedBetIndex >= marketInfo.PossibleBets.Count - 1;
    }

    /// <summary>
    /// Checks if the current selected bet index is the lowest it can go.
    /// </summary>
    /// <returns>True if the selected bet index is at its minimum.</returns>
    public bool IsSelectedBetIndexAtMin()
    {
        return currentSelectedBetIndex <= 0;
    }
}
