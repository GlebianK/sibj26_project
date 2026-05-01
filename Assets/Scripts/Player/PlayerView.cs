using Az7.Utils;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Volume _fxVolume;
    private MeshRenderer[] _renderers;

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

            foreach (var renderer in _renderers)
            {
                for (var i = 0; i < renderer.materials.Length; i++)
                {
                    var material = renderer.materials[i];
                    material.SetFloat("_T", value);
                }
            }

            await UniTask.Yield();

            time += Time.deltaTime;
        }
    }

    public void SetNormal()
    {
        foreach (var renderer in _renderers)
        {
            for (var i = 0; i < renderer.materials.Length; i++)
            {
                var material = renderer.materials[i];
                material.SetFloat("_T", 0f);
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
    }
}
