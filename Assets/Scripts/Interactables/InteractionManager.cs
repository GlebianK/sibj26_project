using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    [SerializeField] private bool isDebugging;
    [SerializeField] private InteractionComponent playerIC;
    [SerializeField, HideInInspector] private InteractableType lastType;

    public bool HandsAreBusy { get; private set; }
    public GameObject ObjectInHands { get; private set; }
    public InteractableMovable ObjectBeingMoved { get; private set; }

    public bool IsInInteraction { get; private set; }


    private void Awake()
    {
        if (isDebugging)
            Debug.Log($"Saved type: {lastType}");

        if (Instance == null)
            Instance = this;

        IsInInteraction = false;
        lastType = InteractableType.None;

        HandsAreBusy = false;

        if (isDebugging)
            Debug.Log($"New type: {lastType}");
    }

    public void OccupyHands(bool shouldOccupy, GameObject objectToOccupyWith = null)
    {
        HandsAreBusy = shouldOccupy;
        ObjectInHands = objectToOccupyWith;
    }

    public void SetMovable(bool shouldSetObject, InteractableMovable objectToSet = null)
    {
        HandsAreBusy = shouldSetObject;
        ObjectBeingMoved = objectToSet;
    }

    public void TryInteract(GameObject interactionInitiator)
    {
        if (isDebugging)
            Debug.Log($"IM: trying to interact... Current Value.Type = {Blackboard.SelectedInteractable.Value.Type}");

        
        if (Blackboard.SelectedInteractable.Value == null)
            return;
        

        IsInInteraction = true;

        bool interactionResult;

        switch (Blackboard.SelectedInteractable.Value.Type)  // TODO: в кейсах прописать действия игрока (изменение стейтов/анимаций)
        {
            case InteractableType.Evironment:
                lastType = Blackboard.SelectedInteractable.Value.Type;
                Blackboard.PlayerStateProperty.Value = PlayerState.Cutscene;
                interactionResult = Blackboard.SelectedInteractable.Value.Interact();
                PrintDebug(interactionResult);
                return;
            case InteractableType.Movable:
                lastType = Blackboard.SelectedInteractable.Value.Type;
                Blackboard.PlayerStateProperty.Value = PlayerState.Pulling;
                interactionResult = Blackboard.SelectedInteractable.Value.Interact(playerIC.BearPushPullPoint);
                PrintDebug(interactionResult);
                return;
            case InteractableType.Togglable:
                lastType = Blackboard.SelectedInteractable.Value.Type;
                interactionResult = Blackboard.SelectedInteractable.Value.Interact();
                PrintDebug(interactionResult);
                return;
            case InteractableType.Item:
                lastType = Blackboard.SelectedInteractable.Value.Type;
                interactionResult = Blackboard.SelectedInteractable.Value.Interact(playerIC.ItemHoldingPoint);
                PrintDebug(interactionResult);
                return;
            case InteractableType.Breakable:
                lastType = Blackboard.SelectedInteractable.Value.Type;
                PlayerController.Instance.Punch();
                interactionResult = Blackboard.SelectedInteractable.Value.Interact();
                PrintDebug(interactionResult);
                return;
            default:
                Debug.Log($"Wrong value for InteractionManager! Value = <color=red>{Blackboard.SelectedInteractable.Value.Type}</color>");
                lastType = InteractableType.None;
                Blackboard.PlayerStateProperty.Value = PlayerState.None;
                IsInInteraction = false;
                return;
        }
    }

    private void PrintDebug(bool res)
    {
        if (!isDebugging)
            return;

        string colorRed = "red";
        string colorGreen = "green";

        string resColor = res ? colorGreen : colorRed;
        Debug.Log($"INTERACTION RESULT: <color={resColor}>{res}</color>");
    }

    public void CompleteInteraction()
    {
        if (isDebugging)
            Debug.Log("IM: <color=yellow>cancelling interaction...</color>");

        if (lastType == InteractableType.None|| !IsInInteraction)
        {
            if (isDebugging)
                Debug.Log("<color=yellow>Trying to cancel interaction with</color> <color=red>NULL</color>");
            return;
        }

        lastType = InteractableType.None;
        IsInInteraction = false;
        Blackboard.PlayerStateProperty.Value = PlayerState.Movement;
    }

    private void OnGUI()
    {
        if (!isDebugging)
            return;

        // Creating text style
        GUIStyle labelStyle = new(GUI.skin.label);
        labelStyle.fontSize = 20;
        labelStyle.fontStyle = FontStyle.Bold;

        // Debug window position and size
        Rect debugRect = new(10, 75, 350, 150);

        // Creating debug area
        GUILayout.BeginArea(debugRect);
        GUILayout.BeginVertical("box");

        // Showing shots and reloads left
        GUIStyle userLabelStyle = new(labelStyle);
        userLabelStyle.normal.textColor = Color.white;

        GUILayout.Label($"Состояние интеракции: {IsInInteraction}", userLabelStyle);

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
