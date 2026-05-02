using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _playerView;
    [SerializeField] private float _smoothSpeed = 10f;
    [SerializeField] private float _offsetMultiplier = 1f;

    private void LateUpdate()
    {
        var offset = _playerView.forward * _offsetMultiplier;
        offset.z = 0f;
        offset = offset.normalized;
        transform.position = Vector3.Lerp(transform.position, _playerView.position + offset, _smoothSpeed * Time.deltaTime);
    }
}
