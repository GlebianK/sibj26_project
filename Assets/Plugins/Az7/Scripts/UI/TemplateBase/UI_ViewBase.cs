using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Az7.UI
{
    public abstract class UI_ViewBase : MonoBehaviour
    {
        public event Action OnShowStarted;
        public event Action OnShowComplete;
        public event Action OnHideStarted;
        public event Action OnHideComplete;

        public abstract UI_ViewKey ViewKey { get; }
        public bool IsVisible { get; protected set; }
        public float AnimationTime { get; set; }
        [field: SerializeField] public float DefaultAnimationTime { get; protected set; } = .2f;
        [field: SerializeField] public bool IgnoreHideAll { get; set; } = false;
        [field: SerializeField] public bool ShowByDefault { get; set; } = false;
        [SerializeField] protected CanvasGroup _canvasGroup;

        private CancellationTokenSource _cts;

        public void ShowImmidiate()
        {
            Cancel();
            OnShowStarted?.Invoke();
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
            IsVisible = true;
            OnShowComplete?.Invoke();
        }

        public void HideImmidiate()
        {
            Cancel();
            OnHideStarted?.Invoke();
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0f;
            IsVisible = false;
            OnHideComplete?.Invoke();
        }

        public UniTask ShowAsync()
        {
            Cancel();
            _cts = new();
            return ViewAnimationAsync(true, _cts.Token, destroyCancellationToken);
        }

        public UniTask HideAsync()
        {
            Cancel();
            _cts = new();
            return ViewAnimationAsync(false, _cts.Token, destroyCancellationToken);
        }

        private async UniTask ViewAnimationAsync(bool show,
            CancellationToken cancellationToken,
            CancellationToken destroyToken)
        {
            var animationTime = AnimationTime > 0f ? AnimationTime : DefaultAnimationTime;
            var startValue = _canvasGroup.alpha;
            var endValue = show ? 1f : 0f;

            if (show)
            {
                _canvasGroup.blocksRaycasts = true;
                IsVisible = true;
                OnShowStarted?.Invoke();
            }
            else
            {
                _canvasGroup.blocksRaycasts = false;
                IsVisible = false;
                OnHideStarted?.Invoke();
            }

            if (startValue != endValue && animationTime > 0f)
            {
                var t = 0f;
                var time = 0f;

                while (t < 1f && !cancellationToken.IsCancellationRequested)
                {
                    t = time / animationTime;
                    time += Time.unscaledDeltaTime;

                    _canvasGroup.alpha = Mathf.Lerp(startValue, endValue, t);

                    await UniTask.Yield();
                    if (destroyToken.IsCancellationRequested) return;
                }
            }

            _canvasGroup.alpha = endValue;

            if (show)
            {
                OnShowComplete?.Invoke();
            }
            else
            {
                OnHideComplete?.Invoke();
            }
        }

        private void Cancel()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
