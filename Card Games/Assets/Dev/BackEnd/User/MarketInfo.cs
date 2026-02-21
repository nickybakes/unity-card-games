using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MarketInfo", menuName = "Scriptable Objects/Market Info")]
public class MarketInfo : ScriptableObject
{
    /// <summary>
    /// List of possible bet values the user can select.
    /// </summary>
    [field: SerializeField] public List<float> PossibleBets { get; private set; }

    /// <summary>
    /// Default selected bet index.
    /// </summary>
    [field: SerializeField] public int DefaultBetIndex { get; private set; }

    /// <summary>
    /// The amount of credits the user starts with.
    /// </summary>
    [field: SerializeField] public float UserStartingCredits { get; private set; }
}
