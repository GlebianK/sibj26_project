using UnityEngine;

public class InteractableTogglable : InteractableBase
{
    [SerializeField] private bool isInteractableOnce;

    [SerializeField] private TogglableBase connectedTogglable;

    [Tooltip("0 - default state")]
    [SerializeField] private GameObject[] objectStates;

    private bool hasBeenInteractedWith;

    protected override void Awake()
    {
        base.Awake();
        hasBeenInteractedWith = false;

        for (int i = 0; i < objectStates.Length; i++)
        {
            if (i == 0)
                objectStates[i].SetActive(true);
            else
                objectStates[i].SetActive(false);
        }
    }

    override public bool Interact()
    {
        if (isDebugging)
            Debug.Log("Toggable: <color=yellow>Trying to interact... </color>");

        if (isInteractableOnce && hasBeenInteractedWith)
            return false;

        base.Interact();
        if (isDebugging)
            Debug.Log("This is the <color=yellow>InteractableTogglable's</color> Interact method ;)");

        hasBeenInteractedWith = true;
        ChangeState();

        return true;
    }   
    
    private void ChangeState()
    {
        if (isDebugging)
            Debug.Log($"{gameObject.name}: <color=green>changing state here =)</color>");
        
        // TODO: логика смены состояний
        foreach (GameObject obj in objectStates)
            obj.SetActive(!obj.activeInHierarchy);

        if (connectedTogglable != null)
            connectedTogglable.ChangeState();

        InteractionManager.Instance.CompleteInteraction();

        if (isDebugging)
            Debug.Log($"Interaction with type {Blackboard.SelectedInteractable.Value.Type}: <color=green>success!</color>");
    }
}
