using Az7.Extensions;
using Cysharp.Threading.Tasks;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public bool AllowMovement { get; set; }
    public PlayerForm Form { get; private set; }

    [SerializeField] private InputActionReference _movementActionReference;
    [SerializeField] private InputActionReference _jumpActionReference;
    [SerializeField] private InputActionReference _toggleFormActionReference;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerConfig _humanConfig;
    [SerializeField] private PlayerConfig _bearConfig;
    [SerializeField] private PlayerView _humanView;
    [SerializeField] private PlayerView _bearView;
    [SerializeField] private ParticleSystem _transformationFX;
    [SerializeField] private bool _allowZMovement = false;
    [SerializeField] private float _movementSpeed = 1.0f;
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private bool _debug;

    private Vector3 _movementDirection;
    private StringBuilder _debugText = new();
    private CancellationTokenSource _cts;

    public void Setup(Vector3 position)
    {
        _characterController.enabled = false;
        transform.position = position;
        _characterController.enabled = true;
        ChangeFormImmidiate(PlayerForm.Human);
    }

    public void ChangeForm(PlayerForm form)
    {
        if (_cts != null) return;
        Form = form;
        _cts = new();
        ChangeFormAsync(form, _cts.Token).Forget();
    }

    public void ChangeFormImmidiate(PlayerForm form)
    {
        Form = form;
        SetControlleSettings(form);
        var currentView = Form == PlayerForm.Human ? _bearView : _humanView;
        var targetView = Form == PlayerForm.Human ? _humanView : _bearView;
        currentView.gameObject.SetActive(false);
        targetView.gameObject.SetActive(true);
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning($"Instance of {GetType()} already created!");
        }

        _movementActionReference.action.performed += Movement_performed;
        _movementActionReference.action.canceled += Movement_canceled;
        _toggleFormActionReference.action.performed += ToggleForm_performed;
    }


    private void Update()
    {
        Move();
    }

    private async UniTaskVoid ChangeFormAsync(PlayerForm form, CancellationToken token)
    {
        var currentView = Form == PlayerForm.Human ? _bearView : _humanView;
        var targetView = Form == PlayerForm.Human ? _humanView : _bearView;

        await currentView.ChangeViewAsync(.3f, true, token);
        currentView.gameObject.SetActive(false);
        SetControlleSettings(form);
        _transformationFX.Play();
        targetView.gameObject.SetActive(true);
        await targetView.ChangeViewAsync(.5f, false, token);

        Cancel();
    }

    private void SetControlleSettings(PlayerForm form)
    {
        var config = form == PlayerForm.Human ? _humanConfig : _bearConfig;
        _movementSpeed = config.Speed;
        _characterController.radius = config.Radius;
        _characterController.height = config.Height;
        _characterController.center = config.Center;
    }

    private void Move()
    {
        if (!AllowMovement) return;
        var gravityForce = new Vector3(0f, _gravity, 0f);
        var movementVelocity = (_movementDirection + gravityForce) * _movementSpeed * Time.deltaTime;
        _characterController.Move(movementVelocity);
    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {
        _movementDirection = obj.ReadValue<Vector2>().Convert3XZ();

        if (!_allowZMovement)
        {
            _movementDirection.z = 0f;
        }

        _movementDirection.Normalize();
    }

    private void Movement_canceled(InputAction.CallbackContext obj)
    {
        _movementDirection = Vector3.zero;
    }

    private void ToggleForm_performed(InputAction.CallbackContext obj)
    {
        var nextForm = Form == PlayerForm.Human ? PlayerForm.Bear : PlayerForm.Human;
        ChangeForm(nextForm);
    }

    private void Cancel()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private void OnGUI()
    {
        if (!_debug) return;

        _debugText.Clear();
        _debugText.Append($"MovementDirection: {_movementDirection}\n");

        var style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(20, 20, 200, 50), _debugText.ToString(), style);
    }
}
