using Az7.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

/// <summary>
/// Компонент, которого повесим на плеера
/// </summary>
public class InteractionComponent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _interactionActionReference;
    [SerializeField] private GameObject itemHoldingPoint;

    [Space, Header("Spere cast info")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _castRadius = .5f;
    [SerializeField] private Vector3 _castOffsetTop;
    [SerializeField] private Vector3 _castOffsetBottom;
    [SerializeField] private bool _drawDebug;

    private Collider[] _results = new Collider[1];

    public GameObject ItemHoldingPoint { get { return itemHoldingPoint; } }

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

    private void OnDestroy()
    {
        _interactionActionReference.action.performed -= Interaction_performed;
    }

    private void FixedUpdate()
    {
        Cast();
    }

    private void Cast()
    {
        var hitCount = Physics.OverlapCapsuleNonAlloc(transform.position + _castOffsetTop, transform.position + _castOffsetBottom,
            _castRadius, _results, _layerMask);

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
        
        InteractionManager.Instance.TryInteract(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!_drawDebug) return;

        Vector3 position1 = transform.position + _castOffsetTop;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position1, _castRadius);

        Vector3 position2 = transform.position + _castOffsetBottom;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position2, _castRadius);

    }
}
