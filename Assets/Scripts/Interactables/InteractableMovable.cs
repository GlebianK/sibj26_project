using UnityEngine;

public class InteractableMovable : InteractableBase
{
    private bool isBeingInteractedWith;

    protected void Awake()
    {
        base.Awake();
        isBeingInteractedWith = false;
    }

    override public bool Interact()
    {
        if (isBeingInteractedWith)
        {
            InteractionManager.Instance.CompleteInteraction();
            return false;
        }

        base.Interact();
        if (isDebugging)
            Debug.Log($"This is the <color=yellow>InteractableEnvironment's</color> Interact method! GO: {gameObject.name}");

        isBeingInteractedWith = true;
        
        return true;
    }
}
