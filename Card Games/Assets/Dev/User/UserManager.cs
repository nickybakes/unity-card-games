using System;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    [SerializeField] private MarketInfo marketInfo;

    private float credits;

    private List<float> possibleBets;

    private int currentSelectedBetIndex;

    public int CurrentSelectedBetIndex
    {
        get
        {
            return CurrentSelectedBetIndex;
        }

        set
        {
            currentSelectedBetIndex = Math.Clamp(value, 0, possibleBets.Count - 1);
        }
    }

    public float CurrentBet
    {
        get
        {
            return possibleBets[currentSelectedBetIndex];
        }
    }

    void Awake()
    {
        GetMarketInfo();
    }

    /// <summary>
    /// Retrieves the market info data. If connecting to an online service,
    /// that would be done here to retrieve the data.
    /// </summary>
    private void GetMarketInfo()
    {
        credits = marketInfo.UserStartingCredits;
        possibleBets = new List<float>(marketInfo.PossibleBets);
        currentSelectedBetIndex = marketInfo.DefaultBetIndex;
    }
}
