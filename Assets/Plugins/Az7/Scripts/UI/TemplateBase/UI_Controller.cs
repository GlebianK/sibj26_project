using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Az7.UI
{
    public class UI_Controller : MonoBehaviour
    {
        [SerializeField] private bool _initializeOnAwake = true;
        [SerializeField] private bool _debugMessages = true;

        protected Dictionary<UI_ViewKey, UI_ViewBase> _views = new();
        private bool _isInitialized = false;

        public void Initialize()
        {
            if (_isInitialized) return;

            var views = FindObjectsByType<UI_ViewBase>();

            foreach (var view in views)
            {
                _views.Add(view.ViewKey, view);
                if (view.ShowByDefault)
                {
                    view.ShowImmidiate();
                }
                else
                {
                    view.HideImmidiate();
                }
            }

            _isInitialized = true;
        }

        public void ShowViewImmidiate(UI_ViewKey key)
        {
            if (_views.TryGetValue(key, out UI_ViewBase view))
            {
                if (view.IsVisible)
                {
                    if (_debugMessages)
                    {
                        Debug.Log($"<color=#65D766>UI</color> View is already visible: {key}");
                    }

                    return;
                }

                view.ShowImmidiate();

                if (_debugMessages)
                {
                    Debug.Log($"<color=#65D766>UI</color> Show: {key}");
                }
            }
            else
            {
                Debug.LogWarning($"<color=#65D766>UI</color> View not found: {key}");
            }
        }

        public void HideViewImmidiate(UI_ViewKey key)
        {
            if (_views.TryGetValue(key, out UI_ViewBase view))
            {
                if (!view.IsVisible)
                {
                    if (_debugMessages)
                    {
                        Debug.Log($"<color=#65D766>UI</color> View is already invisible: {key}");
                    }

                    return;
                }

                view.HideImmidiate();

                if (_debugMessages)
                {
                    Debug.Log($"<color=#65D766>UI</color> Hide: {key}");
                }
            }
            else
            {
                Debug.LogWarning($"<color=#65D766>UI</color> View not found: {key}");
            }
        }

        public async UniTask ShowViewAsync(UI_ViewKey key)
        {
            if (_views.TryGetValue(key, out UI_ViewBase view))
            {
                if (view.IsVisible)
                {
                    if (_debugMessages)
                    {
                        Debug.Log($"<color=#65D766>UI</color> View is already visible: {key}");
                    }

                    return;
                }

                if (_debugMessages)
                {
                    Debug.Log($"<color=#65D766>UI</color> Start show Async: {key}");
                }

                view.AnimationTime = view.DefaultAnimationTime;
                await view.ShowAsync();
            }
            else
            {
                Debug.LogWarning($"<color=#65D766>UI</color> View not found: {key}");
            }
        }

        public async UniTask ShowViewAsync(UI_ViewKey key, float duration)
        {
            if (_views.TryGetValue(key, out UI_ViewBase view))
            {
                if (view.IsVisible)
                {
                    if (_debugMessages)
                    {
                        Debug.Log($"<color=#65D766>UI</color> View is already visible: {key}");
                    }

                    return;
                }

                if (_debugMessages)
                {
                    Debug.Log($"<color=#65D766>UI</color> Start show Async: {key}");
                }

                view.AnimationTime = duration;
                await view.ShowAsync();
            }
            else
            {
                Debug.LogWarning($"<color=#65D766>UI</color> View not found: {key}");
            }
        }

        public async UniTask HideViewAsync(UI_ViewKey key)
        {
            if (_views.TryGetValue(key, out UI_ViewBase view))
            {
                if (!view.IsVisible)
                {
                    if (_debugMessages)
                    {
                        Debug.Log($"<color=#65D766>UI</color> View is already invisible: {key}");
                    }

                    return;
                }

                if (_debugMessages)
                {
                    Debug.Log($"<color=#65D766>UI</color> Start hide Async: {key}");
                }
                view.AnimationTime = view.DefaultAnimationTime;
                await view.HideAsync();
            }
            else
            {
                Debug.LogWarning($"<color=#65D766>UI</color> View not found: {key}");
            }
        }

        public async UniTask HideViewAsync(UI_ViewKey key, float duration)
        {
            if (_views.TryGetValue(key, out UI_ViewBase view))
            {
                if (!view.IsVisible)
                {
                    if (_debugMessages)
                    {
                        Debug.Log($"<color=#65D766>UI</color> View is already invisible: {key}");
                    }

                    return;
                }

                if (_debugMessages)
                {
                    Debug.Log($"<color=#65D766>UI</color> Start hide Async: {key}");
                }
                view.AnimationTime = duration;
                await view.HideAsync();
            }
            else
            {
                Debug.LogWarning($"<color=#65D766>UI</color> View not found: {key}");
            }
        }

        public void HideAll()
        {
            foreach (var view in _views)
            {
                if (view.Value.IsVisible && !view.Value.IgnoreHideAll)
                {
                    view.Value.HideImmidiate();

                    if (_debugMessages)
                    {
                        Debug.Log($"<color=#65D766>UI</color> Hide: {view.Key}");
                    }
                }
            }
        }

        public UI_ViewBase GetView(UI_ViewKey key)
        {
            if (_views.TryGetValue(key, out UI_ViewBase view))
            {
                return view;
            }
            else
            {
                return null;
            }
        }

        protected virtual void AwakeBase()
        {
            if (_initializeOnAwake)
            {
                Initialize();
            }
        }

        protected virtual void OnDestroyBase() { }

        private void Awake()
        {
            AwakeBase();
        }

        private void OnDestroy()
        {
            OnDestroyBase();
        }
    }
}
