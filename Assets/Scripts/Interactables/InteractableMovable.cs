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
        if (isDebugging)
            Debug.Log($"isBeingInteracted with = <color=yellow>{isBeingInteractedWith}</color>");

        if (isBeingInteractedWith)
        {
            transform.parent = null;
            isBeingInteractedWith = false;
            InteractionManager.Instance.CompleteInteraction();
            return false;
        }

        //base.Interact();
        if (isDebugging)
            Debug.Log($"This is the <color=yellow>InteractableEnvironment's</color> Interact method! GO: {gameObject.name}");

        isBeingInteractedWith = true;
        transform.SetParent(newParent.transform);
        transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
        
        return true;
    }
}
