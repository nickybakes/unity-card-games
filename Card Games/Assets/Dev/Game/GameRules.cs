using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameRules", menuName = "Scriptable Objects/GameRules")]
public class GameRules : ScriptableObject
{

    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    [field: SerializeField] public int HandSize { get; private set; }

    [field: SerializeField] public int NumberOfDraws { get; private set; }

    [field: SerializeField] public DeckData deckData { get; private set; }

    [field: SerializeField] public Scene SceneToLoad { get; private set; }
}
