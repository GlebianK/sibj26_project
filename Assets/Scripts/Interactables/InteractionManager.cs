using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    [SerializeField] private bool isDebugging;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void TryInteract(GameObject interactionInitiator)
    {
        if (isDebugging)
            Debug.Log("IM: trying to interact...");

        if (Blackboard.SelectedInteractable.Value == null)
            return;

        switch (Blackboard.SelectedInteractable.Value.Type)  // в кейсах прописать действия игрока (изменение стейтов/анимаций)
        {
            case InteractableBase.InteractableType.Evironment:
                break;
            case InteractableBase.InteractableType.Movable:
                break;
            case InteractableBase.InteractableType.Togglable:
                Blackboard.SelectedInteractable.Value.Interact();
                break;
            case InteractableBase.InteractableType.Item:
                Blackboard.SelectedInteractable.Value.Interact();
                break;
            default:
                Debug.Log($"Wrong value for InteractionManager! Value = <color=red>{Blackboard.SelectedInteractable.Value.Type}</color>");
                break;
        }

        if (isDebugging)
            Debug.Log($"Interaction with type <color=green>{Blackboard.SelectedInteractable.Value.Type}</color>: success!");
    }
}
