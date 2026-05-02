using Az7.Extensions;
using Cysharp.Threading.Tasks;
using System;
using System.Text;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public bool AllowMovement { get; set; } = true;
    //public bool AllowInput { get => _allowInput; set => SetAllowInput(value); }

    public PlayerForm Form { get; private set; } = PlayerForm.Human;

    [SerializeField] private InputActionReference _movementActionReference;
    [SerializeField] private InputActionReference _jumpActionReference;
    [SerializeField] private InputActionReference _toggleFormActionReference;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerConfig _humanConfig;
    [SerializeField] private PlayerConfig _bearConfig;
    [SerializeField] private PlayerView _humanView;
    [SerializeField] private PlayerView _bearView;
    [SerializeField] private Animator _humanAnimator;
    [SerializeField] private Animator _bearAnimator;
    [SerializeField] private Transform _views;
    [SerializeField] private LayerMask _climbingLayerMask;
    [SerializeField] private ParticleSystem _transformationFX;
    [SerializeField] private bool _allowZMovement = false;
    [SerializeField] private float _movementSpeed = 1.0f;
    [SerializeField] private float _rotationSpeed = 12.0f;
    [SerializeField] private float _groundedGravity = -3f;
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _gravityMultiplier = 3f;
    [SerializeField] private float _jumpCooldown = .3f;
    [SerializeField] private bool _debug;

    [Header("Audio")]
    [SerializeField] private AudioSource _transformationSFX;
    [SerializeField] private AudioSource _humanJumpSFX;
    [SerializeField] private AudioSource _BearJumpSFX;
    [SerializeField] private AudioSource _humanStepsSFX;
    [SerializeField] private AudioSource _bearStepsSFX;

    private bool _allowInput = true;
    private Vector3 _movementDirection;
    private Vector3 _velocity;
    private float _jumpTimer;
    private bool _isJump;
    //private bool _isClimbing;
    private Quaternion _targetRotation;
    private StringBuilder _debugText = new();
    private CancellationTokenSource _cts;
    private ClimbingObject _climbingObject;

    public void Setup(Vector3 position)
    {
        _characterController.enabled = false;
        transform.position = position;
        _characterController.enabled = true;
        ChangeFormImmidiate(PlayerForm.Human);
        _targetRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
    }

    public void ChangeForm(PlayerForm form)
    {
        if (_cts != null) return;
        Form = form;
        _cts = new();
        ChangeFormAsync(form, _cts.Token).Forget();
        _transformationSFX.Play();
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

    public void EndClimb()
    {
        _characterController.enabled = false;
        transform.position = _climbingObject.Target.position;
        _characterController.enabled = true;
        //_bearAnimator.applyRootMotion = false;
        SetAllowInput(true);
        //_isClimbing = false;
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
        _jumpActionReference.action.performed += Jump_performed;

        Blackboard.PlayerStateProperty.SkipLatestValueOnSubscribe().Subscribe(value =>
        {
            if (value == PlayerState.Movement || value == PlayerState.Pulling)
            {
                SetAllowInput(true);
            }
            else
            {
                SetAllowInput(false);
            }

            _bearAnimator.SetBool("IsPushing", value == PlayerState.Pulling);
        }).AddTo(this);
    }


    private void OnDestroy()
    {
        _movementActionReference.action.performed -= Movement_performed;
        _movementActionReference.action.canceled -= Movement_canceled;
        _toggleFormActionReference.action.performed -= ToggleForm_performed;
        _jumpActionReference.action.performed -= Jump_performed;
    }

    private void Update()
    {
        CalculateGravity();
        Move();
        Rotation();

        if (_jumpTimer >= 0f)
        {
            _jumpTimer -= Time.deltaTime;
        }

        if (!_isJump && !_characterController.isGrounded)
        {
            _isJump = true;
        }

        var isMoving = _movementDirection.sqrMagnitude > 0f;
        var isJumping = !_characterController.isGrounded;
        var playStepSFX = _isJump && !isJumping;

        if (_humanAnimator.gameObject.activeInHierarchy)
        {
            _humanAnimator.SetBool("IsMoving", isMoving);
            _humanAnimator.SetBool("IsJumping", isJumping);

            if (playStepSFX)
            {
                _humanStepsSFX.Play();
            }
        }

        if (_bearAnimator.gameObject.activeInHierarchy)
        {
            _bearAnimator.SetBool("IsMoving", isMoving);
            _bearAnimator.SetBool("IsJumping", isJumping);

            if (playStepSFX)
            {
                _bearStepsSFX.Play();
            }

            //if (_isClimbing)
            //{
            //    Debug.Log(_bearAnimator.deltaPosition);
            //    _characterController.Move(_bearAnimator.deltaPosition);
            //    _bearAnimator.gameObject.transform.localPosition = Vector3.zero;
            //    _bearAnimator.gameObject.transform.localRotation = Quaternion.identity;
            //}
        }

        if (playStepSFX)
        {
            _isJump = false;
        }
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

    private void CalculateGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0f)
        {
            _velocity.y = _groundedGravity;
        }
        else
        {
            _velocity.y += _gravity * _gravityMultiplier * Time.deltaTime;
        }
    }

    private void Move()
    {
        if (!AllowMovement) return;
        var targetVelocity = _velocity + (_movementDirection * _movementSpeed);

        var zOffset = new Vector3(0f, 0f, transform.position.z);
        _characterController.Move(-zOffset + targetVelocity * Time.deltaTime);
    }

    private void Rotation()
    {
        if (_movementDirection.sqrMagnitude > 0.01f
            && Blackboard.PlayerStateProperty.Value == PlayerState.Movement)
        {
            _targetRotation = Quaternion.LookRotation(_movementDirection, Vector3.up);
        }

        _views.rotation = Quaternion.Slerp(_views.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        if (_allowInput
            && _characterController.isGrounded
            && _jumpTimer <= 0f
            && Blackboard.PlayerStateProperty.Value == PlayerState.Movement)
        //&& Form == PlayerForm.Human)
        {
            var jumpForce = Form == PlayerForm.Human ? _humanConfig.JumpForce : _bearConfig.JumpForce;
            _velocity.y = jumpForce;
            _jumpTimer = _jumpCooldown;

            if (Form == PlayerForm.Human)
            {
                _humanJumpSFX.Play();
            }
            else
            {
                _BearJumpSFX.Play();
            }


            //if (Form == PlayerForm.Human)
            //{
            //    var jumpForce = Form == PlayerForm.Human ? _humanConfig.JumpForce : _bearConfig.JumpForce;
            //    _velocity.y = jumpForce;
            //    _jumpTimer = _jumpCooldown;
            //}
            //else if (Form == PlayerForm.Bear)
            //{
            //    var position = transform.position + _views.forward;
            //    var results = Physics.OverlapSphere(position, .5f, _climbingLayerMask);
            //    if (results.Length > 0)
            //    {
            //        if (results[0].attachedRigidbody.gameObject.TryGetComponent<ClimbingObject>(out var climbingObject))
            //        {
            //            //_bearAnimator.applyRootMotion = true;
            //            _climbingObject = climbingObject;
            //            _isClimbing = true;
            //            SetAllowInput(false);
            //            _bearAnimator.SetTrigger("Climb");
            //        }
            //    }
            //    else
            //    {
            //        var jumpForce = Form == PlayerForm.Human ? _humanConfig.JumpForce : _bearConfig.JumpForce;
            //        _velocity.y = jumpForce;
            //        _jumpTimer = _jumpCooldown;
            //    }
            //}
        }
    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {
        if (!_allowInput) return;

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
        if (!_allowInput || Blackboard.PlayerStateProperty.Value != PlayerState.Movement) return;
        var nextForm = Form == PlayerForm.Human ? PlayerForm.Bear : PlayerForm.Human;
        ChangeForm(nextForm);
    }

    private void Cancel()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private void SetAllowInput(bool value)
    {
        if (!value)
        {
            _movementDirection = Vector3.zero;
        }

        _allowInput = value;
    }

    private void OnGUI()
    {
        if (!_debug) return;

        _debugText.Clear();
        _debugText.Append($"MovementDirection: {_movementDirection}\n");
        _debugText.Append($"Velocity: {_velocity}\n");
        _debugText.Append($"Grounded: {_characterController.isGrounded}\n");

        var style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(20, 20, 200, 50), _debugText.ToString(), style);
    }
}
