using UnityEngine;

public class InteractableMovable : InteractableBase
{
    private bool isBeingInteractedWith;

    override protected void Awake()
    {
        base.Awake();
        isBeingInteractedWith = false;
    }

    override public bool Interact(GameObject newParent)
    {
        if (isBeingInteractedWith)
        {
            transform.parent = null;
            InteractionManager.Instance.CompleteInteraction();
            return false;
        }

        base.Interact();
        if (isDebugging)
            Debug.Log($"This is the <color=yellow>InteractableEnvironment's</color> Interact method! GO: {gameObject.name}");

        isBeingInteractedWith = true;
        transform.SetParent(newParent.transform);
        
        return true;
    }
}
