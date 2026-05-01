using UnityEngine;

public class InteractableItem : InteractableBase
{
    public bool IsBeingCarried { get; private set; }

    override protected void Awake()
    {
        base.Awake();
        IsBeingCarried = false;
    }

    override public void Interact()
    {
        base.Interact();
        
        if (isDebugging)
            Debug.Log($"This is the <color=yellow>InteractableItem's</color> Interact method! GO: {gameObject.name}");

        IsBeingCarried = !IsBeingCarried;
    }
}
