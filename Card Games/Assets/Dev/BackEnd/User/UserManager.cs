using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    /// <summary>
    /// The current User's info.
    /// </summary>
    public static UserManager user;

    [SerializeField] private MarketInfo marketInfo;

    private float balance;

    private int currentSelectedBetIndex;

    private float winnings;

    private CultureInfo culture;

    public int CurrentSelectedBetIndex
    {
        get
        {
            return currentSelectedBetIndex;
        }
    }

    public float CurrentBet
    {
        get
        {
            return marketInfo.PossibleBets[currentSelectedBetIndex];
        }
    }

    public int NumberPossibleBets
    {
        get
        {
            return marketInfo.PossibleBets.Count;
        }
    }

    public float Balance
    {
        get
        {
            return balance;
        }
    }

    public float Winnings
    {
        get
        {
            return winnings;
        }
    }

    public CultureInfo Culture
    {
        get
        {
            return culture;
        }
    }

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

    public float GetPossibleBet(int index)
    {
        return marketInfo.PossibleBets[index];
    }

    public void AwardWinnings(float _winnings)
    {
        winnings = _winnings;
        balance += winnings;
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

    public bool IsBalanceHighEnoughToBet()
    {
        return balance >= CurrentBet;
    }

    public bool IsSelectedBetIndexAtMax()
    {
        return currentSelectedBetIndex >= marketInfo.PossibleBets.Count - 1;
    }

    public bool IsSelectedBetIndexAtMin()
    {
        return currentSelectedBetIndex <= 0;
    }
}
