using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRulesBase : ScriptableObject
{
    [Header("Base Game Rules")]
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    [field: SerializeField] public int NumberOfHands { get; private set; }

    [field: SerializeField] public List<DeckData> Decks { get; private set; }

    [field: SerializeField] public SceneIndex SceneToLoad { get; private set; }
}
