using Az7.UI;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : UI_ViewBase
{
    public override UI_ViewKey ViewKey => UI_ViewKey.MainMenu;

    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        _startGameButton.onClick.AddListener(() => { Blackboard.StartGame.Execute(); });
        _quitButton.onClick.AddListener(() => { Blackboard.Quit.Execute(); });
    }

    private void OnDestroy()
    {
        _startGameButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();
    }
}
