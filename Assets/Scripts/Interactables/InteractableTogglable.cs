using UnityEngine;

public class InteractableTogglable : InteractableBase
{
    [SerializeField] private bool isInteractableOnce;

    private bool hasBeenInteractedWith;

    protected override void Awake()
    {
        base.Awake();
        hasBeenInteractedWith = false;
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
        Debug.Log($"{gameObject.name}: <color=green>changing state here =)</color>");

        // TODO: логика смены состояний
    }
}
