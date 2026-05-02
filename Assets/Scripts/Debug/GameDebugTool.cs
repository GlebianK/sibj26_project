using UnityEngine;
using UnityEngine.InputSystem;

public class GameDebugTool : MonoBehaviour
{
    [SerializeField] private InputActionReference _startGameActionReference;
    [SerializeField] private InputActionReference _endGameActionReference;
    [SerializeField] private InputActionReference _punchTestActionReference;

    private void Awake()
    {
        _startGameActionReference.action.performed += StartGame;
        _endGameActionReference.action.performed += EndGame;
        _punchTestActionReference.action.performed += TestPunch;
    }


    private void OnDestroy()
    {
        _startGameActionReference.action.performed -= StartGame;
        _endGameActionReference.action.performed -= EndGame;
        _punchTestActionReference.action.performed -= TestPunch;
    }

    private void TestPunch(InputAction.CallbackContext obj)
    {
        PlayerController.Instance.Punch();
    }

    private void StartGame(InputAction.CallbackContext obj)
    {
        Blackboard.StartGame.Execute();
    }

    private void EndGame(InputAction.CallbackContext obj)
    {
        Blackboard.EndGame.Execute();
    }
}
