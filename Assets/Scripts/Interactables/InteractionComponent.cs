using Az7.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

/// <summary>
/// Компонент, которого повесим на плеера
/// </summary>
public class InteractionComponent : MonoBehaviour
{
    [SerializeField] private InputActionReference _interactionActionReference;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _castRadius = .5f;
    [SerializeField] private Vector3 _castOffset;
    [SerializeField] private bool _drawDebug;

    private Collider[] _results = new Collider[1];

    private void Awake()
    {
        _interactionActionReference.action.performed += Interaction_performed;

        Blackboard.SelectedInteractable.SkipLatestValueOnSubscribe().Subscribe(value =>
        {
            if (value != null)
            {
                Debug.Log($"Интерактабл {value.name} детектед!");
            }
        }).AddTo(this);
    }

    private void FixedUpdate()
    {
        Cast();
    }

    private void Cast()
    {
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position + _castOffset, _castRadius, _results, _layerMask);

        if (hitCount == 0)
        {
            Blackboard.SelectedInteractable.Value = null;
        }
        else
        {
            var targetObject = _results[0].attachedRigidbody != null
                ? _results[0].attachedRigidbody.gameObject : _results[0].gameObject;

            if (targetObject.TryGetComponent<InteractableBase>(out var interactable)
                && Blackboard.SelectedInteractable.Value != interactable)
            {
                Blackboard.SelectedInteractable.Value = interactable;
            }
        }
    }

    private void Interaction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("ВЗАИМОДЕЙСТВИЕ Ж!");
    }

    private void OnDrawGizmos()
    {
        if (!_drawDebug) return;

        var position = transform.position + _castOffset;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, _castRadius);
    }
}
