using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FinishGame : TogglableBase
{
    [SerializeField] private CanvasGroup canvasGroupMain;
    [SerializeField] private CanvasGroup canvasGroupText;
    [SerializeField] private Button finishButton;

    [SerializeField] private float fadeDuration;

    private void Start()
    {
        canvasGroupMain.alpha = 0.0f;
        canvasGroupMain.blocksRaycasts = false;
        canvasGroupMain.interactable = false;

        finishButton.interactable = false;
    }

    public override void ChangeState()
    {
        FinishThisGame();        
    }

    private void FinishThisGame()
    {
        MainFadeIn();
        PlayerController.Instance.AllowMovement = false;
        Blackboard.PlayerStateProperty.Value = PlayerState.None;
    }

    private void MainFadeIn()
    {
        Debug.LogWarning("Main fade starts!");
        canvasGroupMain.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            canvasGroupMain.blocksRaycasts = true;
            canvasGroupMain.interactable = true;
            Debug.LogWarning("Main fade ends!");
            SubFadeIn();
        });
    }

    private void SubFadeIn()
    {
        Debug.LogWarning("Sub fade starts!");
        canvasGroupText.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            Debug.LogWarning("Sub fade ends!");
            finishButton.interactable = true;
        });
    }

    public void OnClickFinish()
    {
        Application.Quit();
    }
}
