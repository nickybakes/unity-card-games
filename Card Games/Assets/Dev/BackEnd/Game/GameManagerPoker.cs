using UnityEngine;

public class GameManagerPoker : GameManagerBase
{

    protected GameRulesPoker gameRulesPoker;

    public GameRulesPoker gameRulesOverrive;

    private float timeInGame;

    private void Start()
    {
        LoadGameRules(gameRulesOverrive);
    }

    public override void LoadGameRules(GameRulesBase _gameRules)
    {
        gameRules = _gameRules;
        gameRulesPoker = (GameRulesPoker)_gameRules;
        SetupHands();
    }

    public override void StartGame()
    {
        Debug.Log("Start Game");
        StartRound();
    }

    public override void StartRound()
    {
        base.StartRound();
        DrawCardsToHand(0, 0, gameRulesPoker.HandSize);
        FinishTurn();
    }

    public void Update()
    {
        timeInGame += Time.deltaTime;
        if (timeInGame > 3 && !gameStarted)
        {
            gameStarted = true;
            StartGame();
        }
    }
}
