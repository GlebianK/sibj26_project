using UniRx;
using UnityEngine;

public abstract class PrefsSettingHandlerBase : MonoBehaviour
{
    public ReactiveProperty<float> Value { get; private set; } = new();

    [SerializeField, Range(0f, 1f)] protected float _value;
    [SerializeField] protected float _defaultValue;
    [SerializeField] protected UserSettingsKey _key;
    [SerializeField] private bool _initializeOnAwake = true;

    private bool _isInitialized;

    public void Initialize()
    {
        if (_isInitialized) return;

        //Value = new();

        Value.SkipLatestValueOnSubscribe().Subscribe(value =>
        {
            value = Mathf.Clamp01(value);
            SetValue(value);
            Store(value);
        }).AddTo(this);

        Value.SetValueAndForceNotify(Get());

        _isInitialized = true;
    }

    protected abstract void SetValue(float value);

    protected void Store(float value)
    {
        PlayerPrefs.SetFloat(_key.ToString(), value);
    }

    protected float Get()
    {
        return PlayerPrefs.GetFloat(_key.ToString(), _defaultValue);
    }

    private void Awake()
    {
        if (_initializeOnAwake)
        {
            Initialize();
        }
    }

    private void OnValidate()
    {
        if (_isInitialized)
        {
            Value.Value = _value;
        }
    }
}
