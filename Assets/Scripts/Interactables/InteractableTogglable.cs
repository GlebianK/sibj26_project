using UnityEngine;

public class InteractableTogglable : InteractableBase
{
    [SerializeField] protected bool isInteractableOnce;

    [SerializeField] protected TogglableBase connectedTogglable;

    [Tooltip("0 - default state")]
    [SerializeField] protected GameObject[] objectStates;

    protected bool hasBeenInteractedWith;

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

        if (connectedTogglable.IsChangingState)
            return false;

        base.Interact();
        if (isDebugging)
            Debug.Log("This is the <color=yellow>InteractableTogglable's</color> Interact method ;)");

        ChangeState();

        return true;
    }

    virtual protected void ChangeState()
    {
        if (isDebugging)
            Debug.Log($"{gameObject.name}: <color=green>changing state here =)</color>");
        
        foreach (GameObject obj in objectStates)
            obj.SetActive(!obj.activeInHierarchy);

        if (connectedTogglable != null)
            connectedTogglable.ChangeState();

        InteractionManager.Instance.CompleteInteraction();

        if (isDebugging)
            Debug.Log($"Interaction with type {Blackboard.SelectedInteractable.Value.Type}: <color=green>success!</color>");
    }
}
