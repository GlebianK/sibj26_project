using Az7.Utils;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Volume _fxVolume;
    private MeshRenderer[] _renderers;
    private SkinnedMeshRenderer _meshRenderer;

    public async UniTask ChangeViewAsync(float animationTime, bool forward, CancellationToken token)
    {
        var startValue = forward ? 0f : 1f;
        var endValue = forward ? 1f : 0f;

        var t = 0f;
        var time = 0f;

        while (time <= animationTime && !token.IsCancellationRequested)
        {
            t = time / animationTime;
            t = EaseFunctions.EaseInOutCubic(t);
            var value = Mathf.Lerp(startValue, endValue, t);
            _fxVolume.weight = value;

            SetValue(value);

            await UniTask.Yield();

            time += Time.deltaTime;
        }
    }

    public void SetNormal()
    {
        SetValue(0f);
    }

    private void SetValue(float value)
    {
        if (_renderers != null && _renderers.Length > 0)
        {
            foreach (var renderer in _renderers)
            {
                for (var i = 0; i < renderer.materials.Length; i++)
                {
                    var material = renderer.materials[i];
                    material.SetFloat("_T", value);
                }
            }
        }

        if(_meshRenderer != null)
        {
            for (var i = 0; i < _meshRenderer.materials.Length; i++)
            {
                var material = _meshRenderer.materials[i];
                material.SetFloat("_T", value);
            }
        }
    }

    private void Awake()
    {
        FindRenderers();
    }

    private void FindRenderers()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
}
