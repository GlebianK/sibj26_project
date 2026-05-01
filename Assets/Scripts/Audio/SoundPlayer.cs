using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private float _defaultFadeInTime = 0f;
    [SerializeField] private float _defaultFadeOutTime = 1f;

    private AudioSource _source;
    private float _volume;
    private CancellationTokenSource _cts;

    public void Play()
    {
        Cancel();
        _cts = new CancellationTokenSource();
        ToggleMusicAsync(_defaultFadeInTime, true, _cts.Token).Forget();
    }

    public void Play(float fadeInTime)
    {
        Cancel();
        _cts = new CancellationTokenSource();
        ToggleMusicAsync(fadeInTime, true, _cts.Token).Forget();
    }

    public void Stop()
    {
        Cancel();
        _cts = new CancellationTokenSource();
        ToggleMusicAsync(_defaultFadeOutTime, false, _cts.Token).Forget();
    }

    public void Stop(float fadeOutTime)
    {
        Cancel();
        _cts = new CancellationTokenSource();
        ToggleMusicAsync(fadeOutTime, false, _cts.Token).Forget();
    }

    private async UniTaskVoid ToggleMusicAsync(float fadeTime, bool forward, CancellationToken token)
    {
        var startValue = forward ? 0f : _volume;
        var endValue = forward ? _source.volume : 0f;

        if (fadeTime <= 0f)
        {
            _source.volume = endValue;
            return;
        }

        var t = 0f;
        var time = 0f;

        while (t <= 1f && !token.IsCancellationRequested)
        {
            t = time / fadeTime;
            _source.volume = Mathf.Lerp(startValue, endValue, t);

            await UniTask.Yield();

            time += Time.deltaTime;
        }
    }

    private void Cancel()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _volume = _source.volume;
    }
}
