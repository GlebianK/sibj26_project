using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueHandler : MonoBehaviour
{
    public static DialogueHandler Instance { get; private set; }

    [SerializeField] private InputActionReference _skipActionReference;

    private Dictionary<CharacterKey, CharacterDialogueWindow> _actorsMap = new();
    private DialogueSequenceConfig _currentDialogue;
    private int _eventIndex;
    private CharacterDialogueWindow _activeActor;
    private CancellationTokenSource _cts;

    public void Play(DialogueSequenceConfig dialogue)
    {
        if (Blackboard.PlayerStateProperty.Value != PlayerState.Movement) return;

        Blackboard.PlayerStateProperty.Value = PlayerState.Dialogue;
        _currentDialogue = dialogue;
        _eventIndex = -1;
        ShowNextEventAsync(destroyCancellationToken).Forget();
    }

    public void EndDialogue()
    {
        //Debug.Log("End dialogue");
        Cancel();
        _currentDialogue = null;
        _activeActor?.HideAsync();
        Blackboard.PlayerStateProperty.Value = PlayerState.Movement;
    }

    public void StopAndReset()
    {
        //Debug.Log("Stop and reset");
        Cancel();
        _currentDialogue = null;
        _activeActor?.HideImmidiate();
        Blackboard.PlayerStateProperty.Value = PlayerState.Movement;
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

        _skipActionReference.action.performed += Skip_performed;

        FindActors();
    }

    private void Skip_performed(InputAction.CallbackContext obj)
    {
        ShowNextEvent();
    }

    private void ShowNextEvent()
    {
        if (_cts != null || _currentDialogue == null) return;
        //Debug.Log("Show next event");
        _cts = new();
        ShowNextEventAsync(_cts.Token).Forget();
    }

    private async UniTaskVoid ShowNextEventAsync(CancellationToken token)
    {
        if (_currentDialogue == null)
        {
            Cancel();
            return;
        }

        if (_eventIndex + 1 >= _currentDialogue.Events.Length)
        {
            EndDialogue();
        }
        else
        {
            if (_activeActor != null)
            {
                await _activeActor.HideAsync();
            }

            _eventIndex++;
            var nextEvent = _currentDialogue.Events[_eventIndex];

            if (_actorsMap.TryGetValue(nextEvent.Actor, out var actor))
            {
                _activeActor = actor;
                actor.SetText(nextEvent.Text);

                await actor.ShowAsync();

                Cancel();
            }
            else
            {
                Debug.LogError("Actor is missing!");
            }
        }
    }

    private void FindActors()
    {
        var actorObjects = FindObjectsByType<CharacterDialogueWindow>();

        foreach (var item in actorObjects)
        {
            _actorsMap.Add(item.CharacterKey, item);
        }
    }

    private void Cancel()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }
}
