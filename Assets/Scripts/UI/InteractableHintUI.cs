using Az7.UI;
using UniRx;
using UnityEngine;

public class InteractableHintUI : UI_ViewBase
{
    public override UI_ViewKey ViewKey => UI_ViewKey.InteractablesHint;

    [SerializeField] private CanvasGroup _hintCanvasGroup;
    [SerializeField] private float _animationTime = .2f;
    

    private float _timer;
    private bool _isShown;

    private void Awake()
    {
        Blackboard.SelectedInteractable.SkipLatestValueOnSubscribe().Subscribe(value =>
        {
            _isShown = value != null;
            _timer = 0f;
        }).AddTo(this);
    }

    private void Update()
    {
        if (!IsVisible) return;

        var t = _timer / _animationTime;

        if (t > 1f) return;

        _timer += Time.deltaTime;

        t = Mathf.Clamp01(t);

        var targetValue = _isShown ? 1f : 0f;
        _hintCanvasGroup.alpha = Mathf.Lerp(_hintCanvasGroup.alpha, targetValue, t);
    }

}
