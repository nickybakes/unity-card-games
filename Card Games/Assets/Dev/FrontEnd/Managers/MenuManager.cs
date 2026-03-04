using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private LoadingPanel loadingPanel;

    [SerializeField] private TextDisplay totalBalanceText;

    [SerializeField] private TextDisplay gameDescriptionText;
    [SerializeField] private TextDisplay placeholderDescriptionText;

    [SerializeField] private GameObject uiRaycastShield;

    private bool gameHasBeenChosen = false;

    public void HideGameDescription()
    {
        if (!gameHasBeenChosen)
        {
            gameDescriptionText.Hide();
            placeholderDescriptionText.Show();
        }
    }

    public void ShowGameDescription(GameChoice gameChoice)
    {
        placeholderDescriptionText.Hide();
        gameDescriptionText.SetText(GameCollectionManager.collection.GetGameDescription(gameChoice), true);
        gameDescriptionText.Show();
    }

    public void PlayGameSubmitted(GameChoice gameChoice)
    {
        gameHasBeenChosen = true;
        GameCollectionManager.collection.CurrentlyChosenGame = gameChoice;
        loadingPanel.GoToChosenGameScene();
    }

    public void QuitSubmitted()
    {
        AppManager.app.QuitApp();
    }

    public void UpdateElements()
    {
        if (UserManager.user == null)
            return;

        totalBalanceText.SetText(ParseAsCurrency(UserManager.user.Balance));
    }

    public void DepositAmount(int amount)
    {
        UserManager.user.DepositAmount(amount);
        UpdateElements();
    }

    private string ParseAsCurrency(float amount)
    {
        return amount.ToString("C", UserManager.user.Culture);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateElements();
        gameDescriptionText.Hide();
    }
}
