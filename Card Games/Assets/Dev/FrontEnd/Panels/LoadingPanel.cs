using TMPro;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private bool isInGameplayScene;
    [SerializeField] private TextMeshProUGUI gameNameText;

    private bool goingToGameScene;

    void Awake()
    {
        if (isInGameplayScene)
        {
            SetGameNameText();
            animator.SetTrigger("EnterGame");
        }
        else
        {
            animator.SetTrigger("EnterMenu");
        }
    }

    public void GoToMenuScene()
    {
        goingToGameScene = false;
        animator.SetTrigger("GoToMenu");
    }

    public void GoToChosenGameScene()
    {
        goingToGameScene = true;
        SetGameNameText();
        animator.SetTrigger("GoToGame");
    }

    public void GoToAnimationFinished()
    {
        if (goingToGameScene)
        {
            GameCollectionManager.collection.LoadChosenGameScene();
        }
        else
        {
            GameCollectionManager.collection.LoadGameMenuScene();
        }
    }

    private void SetGameNameText()
    {
        if (GameCollectionManager.collection == null)
            return;

        gameNameText.text = GameCollectionManager.collection.GetGameName();
    }
}
