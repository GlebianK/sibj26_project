using DG.Tweening;
using UnityEngine;

public class InteractableEnvironment : InteractableBase
{
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveDuration;

    public override bool Interact()
    {
        base.Interact();
        if (isDebugging)
            Debug.Log("This is the <color=yellow>InteractableEnvironment's</color> Interact method ;)");

        return true;
    }

    
}
