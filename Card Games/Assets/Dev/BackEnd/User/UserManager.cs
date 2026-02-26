using System;
using System.Collections.Generic;
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

    public int CurrentSelectedBetIndex
    {
        get
        {
            return CurrentSelectedBetIndex;
        }

        set
        {
            currentSelectedBetIndex = Math.Clamp(value, 0, marketInfo.PossibleBets.Count - 1);
        }
    }

    public float CurrentBet
    {
        get
        {
            return marketInfo.PossibleBets[currentSelectedBetIndex];
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

        currentSelectedBetIndex++;
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

        currentSelectedBetIndex--;
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
