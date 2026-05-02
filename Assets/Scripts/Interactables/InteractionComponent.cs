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
    [SerializeField] private GameObject bearPushPullPoint;
    [SerializeField] private GameObject playerView;

    [Space, Header("Spere cast info")]
    [SerializeField] private LayerMask _layerMaskZ;
    [SerializeField] private LayerMask _layerMaskX;
    [SerializeField] private float _castRadius = .5f;
    [SerializeField] private float _castRadiusSphere = .25f;
    [SerializeField] private Vector3 _castOffsetTop;
    [SerializeField] private Vector3 _castOffsetBottom;
    [SerializeField] private float _castOffsetForward;
    [SerializeField] private float _castOffsetForwardY;
    [SerializeField] private bool _drawDebug;

    private Collider[] _results = new Collider[1];

    private int hitCountsCapsule;
    private int hitCountsSphere;
    private Vector3 resultingOffsetForward;

    public GameObject ItemHoldingPoint { get { return itemHoldingPoint; } }
    public GameObject BearPushPullPoint { get { return bearPushPullPoint; } }

    private void Awake()
    {
        hitCountsCapsule = 0;
        hitCountsSphere = 0;

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
        if (PlayerController.Instance.Form == PlayerForm.Human)
            CastCapsule();
        else
            CastSphere();
    }

    private void CastCapsule()
    {
        hitCountsSphere = 0;
        hitCountsCapsule = Physics.OverlapCapsuleNonAlloc(transform.position + _castOffsetTop, transform.position + _castOffsetBottom,
            _castRadius, _results, _layerMaskZ);

        if (hitCountsCapsule == 0 && hitCountsSphere == 0)
        {
            Blackboard.SelectedInteractable.Value = null;
        }
        else if (hitCountsCapsule > 0)
        {
            var targetObject = _results[0].attachedRigidbody != null
                ? _results[0].attachedRigidbody.gameObject : _results[0].gameObject;

            if (targetObject.TryGetComponent<InteractableBase>(out var interactable)
                && Blackboard.SelectedInteractable.Value != interactable)
            {
                /*
                if (_drawDebug)
                    Debug.Log($"Human form required: {interactable.IsInteractableByHuman}, cur.form: {PlayerController.Instance.Form}");
                */

                if ((interactable.IsInteractableByHuman && PlayerController.Instance.Form == PlayerForm.Human) ||
                    (!interactable.IsInteractableByHuman && PlayerController.Instance.Form == PlayerForm.Bear))
                {
                    Blackboard.SelectedInteractable.Value = interactable;
                }
                else
                    Blackboard.SelectedInteractable.Value = null;
            }
        }
    }

    private void CastSphere()
    {
        hitCountsCapsule = 0;
        resultingOffsetForward = playerView.transform.forward * _castOffsetForward;
        resultingOffsetForward.y = _castOffsetForwardY;
        hitCountsSphere = Physics.OverlapSphereNonAlloc(transform.position + resultingOffsetForward,
            _castRadius, _results, _layerMaskX);

        if (hitCountsCapsule == 0 && hitCountsSphere == 0)
        {
            Blackboard.SelectedInteractable.Value = null;
        }
        else if (hitCountsSphere > 0)
        {
            var targetObject = _results[0].attachedRigidbody != null
                ? _results[0].attachedRigidbody.gameObject : _results[0].gameObject;

            if (targetObject.TryGetComponent<InteractableBase>(out var interactable)
                && Blackboard.SelectedInteractable.Value != interactable)
            {
                /*
                if (_drawDebug)
                    Debug.Log($"Human form required: {interactable.IsInteractableByHuman}, cur.form: {PlayerController.Instance.Form}");
                */

                if ((interactable.IsInteractableByHuman && PlayerController.Instance.Form == PlayerForm.Human) ||
                    (!interactable.IsInteractableByHuman && PlayerController.Instance.Form == PlayerForm.Bear))
                {
                    Blackboard.SelectedInteractable.Value = interactable;
                }
                else
                    Blackboard.SelectedInteractable.Value = null;
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

        Vector3 temp = playerView.transform.forward * _castOffsetForward;
        temp.y = _castOffsetForwardY;
        Vector3 position3 = transform.position + temp;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position3, _castRadiusSphere);

    }
}
