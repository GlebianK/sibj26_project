using UnityEngine;

public class InteractableMovable : InteractableBase
{
    override public void Interact()
    {
        base.Interact();
        if (isDebugging)
            Debug.Log($"This is the <color=yellow>InteractableEnvironment's</color> Interact method! GO: {gameObject.name}");
    }
}
