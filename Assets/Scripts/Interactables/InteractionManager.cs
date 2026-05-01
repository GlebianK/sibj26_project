using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    [SerializeField] private bool isDebugging;

    public bool IsInInteraction { get; private set; }

    private InteractableType lastType;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        IsInInteraction = false;
        lastType = InteractableType.None;
    }

    public void TryInteract(GameObject interactionInitiator)
    {
        if (isDebugging)
            Debug.Log("IM: trying to interact...");

        if (Blackboard.SelectedInteractable.Value == null || IsInInteraction)
            return;

        IsInInteraction = true;

        bool interactionResult;

        switch (Blackboard.SelectedInteractable.Value.Type)  // TODO: в кейсах прописать действия игрока (изменение стейтов/анимаций)
        {
            case InteractableType.Evironment:       // СДЕЛАТЬ АСИНК И ЖДАТЬ, ПОКА IsInInteraction НЕ СТАНЕТ FALSE?
                lastType = Blackboard.SelectedInteractable.Value.Type;
                interactionResult = Blackboard.SelectedInteractable.Value.Interact();
                PrintDebug(interactionResult);
                return;
            case InteractableType.Movable:
                lastType = Blackboard.SelectedInteractable.Value.Type;
                return;
            case InteractableType.Togglable:
                lastType = Blackboard.SelectedInteractable.Value.Type;
                interactionResult = Blackboard.SelectedInteractable.Value.Interact();
                PrintDebug(interactionResult);
                return;
            case InteractableType.Item:
                lastType = Blackboard.SelectedInteractable.Value.Type;
                interactionResult = Blackboard.SelectedInteractable.Value.Interact();
                return;
            default:
                Debug.Log($"Wrong value for InteractionManager! Value = <color=red>{Blackboard.SelectedInteractable.Value.Type}</color>");
                lastType = InteractableType.None;
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
        Debug.Log($"<color={resColor}>{res}</color>");
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

        switch (Blackboard.SelectedInteractable.Value.Type)  // TODO: в кейсах прописать действия игрока (изменение стейтов/анимаций)
        {
            case InteractableType.Evironment:
                break;
            case InteractableType.Movable:
                break;
            case InteractableType.Togglable:
                IsInInteraction = false;
                break;
            case InteractableType.Item:
                break;
            default:
                Debug.Log($"Wrong value for InteractionManager! Value = <color=red>{Blackboard.SelectedInteractable.Value.Type}</color>");
                IsInInteraction = false;
                break;
        }

        lastType = InteractableType.None;

        IsInInteraction = false;

        // TODO: смена стейта игрока
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
