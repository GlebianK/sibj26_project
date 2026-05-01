using Az7.UI;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : UI_ViewBase
{
    public override UI_ViewKey ViewKey => UI_ViewKey.MainMenu;

    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private PrefsVolumeSettingHandler _volumeSettingHandler;

    private void Awake()
    {
        _startGameButton.onClick.AddListener(() => { Blackboard.StartGame.Execute(); });
        _quitButton.onClick.AddListener(() => { Blackboard.Quit.Execute(); });

        _volumeSlider.value = _volumeSettingHandler.Value.Value;
        _volumeSlider.onValueChanged.AddListener(value => _volumeSettingHandler.Value.Value = value);
    }

    private void OnDestroy()
    {
        _startGameButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();
        _volumeSlider.onValueChanged.RemoveAllListeners();
    }
}
