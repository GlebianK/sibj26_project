using Az7.Extensions;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference _movementActionReference;
    [SerializeField] private InputActionReference _jumpActionReference;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private bool _allowZMovement = false;
    [SerializeField] private float _movementSpeed = 1.0f;
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private bool _debug;

    private Vector3 _movementDirection;
    private StringBuilder _debugText = new();

    private void Awake()
    {
        _movementActionReference.action.performed += Movement_performed;
        _movementActionReference.action.canceled += Movement_canceled;
    }

    private void Update()
    {
        _movementDirection.y = _gravity;
        var movementVelocity = _movementDirection * _movementSpeed * Time.deltaTime;
        _characterController.Move(movementVelocity);
    }

    private void Movement_canceled(InputAction.CallbackContext obj)
    {
        _movementDirection = Vector2.zero;
    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {
        _movementDirection = obj.ReadValue<Vector2>().Convert3XZ();

        if (!_allowZMovement)
        {
            _movementDirection.z = 0f;
        }
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
