using UnityEngine;

public class InteractableMovable : InteractableBase
{
    public bool CanBePushed { get; private set; } // true - можно толкать, false - упирается в препятствие
    
    private bool isBeingInteractedWith;

    override protected void Awake()
    {
        base.Awake();
        isBeingInteractedWith = false;
        CanBePushed = true;
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
        Vector3 position = transform.position;
        position.y = newParent.transform.position.y;
        transform.position = position;

        transform.SetParent(newParent.transform);

        return true;
    }
}
