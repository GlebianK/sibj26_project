using UnityEngine;

public class InteractableMovable : InteractableBase
{
    public bool CanBePushed { get; private set; } // true - можно толкать, false - упирается в препятствие
    public int PushDirection { get; private set; } // -1 - слева, 1 - справа, 0 - не связан с игроком
    
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
            Debug.Log($"Start of method. isBeingInteractedWith = <color=yellow>{isBeingInteractedWith}</color>");

        if (isBeingInteractedWith)
        {
            transform.parent = null;
            isBeingInteractedWith = false;
            InteractionManager.Instance.SetMovable(false, null);
            InteractionManager.Instance.CompleteInteraction();
            SetPushDirection(false);

            if (isDebugging)
                Debug.Log($"End of method. isBeingInteractedWith = <color=yellow>{isBeingInteractedWith}</color>");

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

        InteractionManager.Instance.SetMovable(true, this);
        SetPushDirection(true);

        if (isDebugging)
            Debug.Log($"End of method. isBeingInteractedWith = <color=yellow>{isBeingInteractedWith}</color>");

        return true;
    }

    private void SetPushDirection(bool shouldSetDirection)
    {
        if (!shouldSetDirection)
        {
            PushDirection = 0;

            if (isDebugging)
                Debug.Log($"No pushing! Direction: <color=yellow>{PushDirection}</color>");

            return;
        }

        if (transform.position.x > transform.parent.position.x)
            PushDirection = 1;
        else PushDirection = -1;

        if (isDebugging)
            Debug.Log($"Pushing! Direction: <color=green>{PushDirection}</color>");
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Obstacle"))
        {
            if (isDebugging)
                Debug.Log($"Obstacle DETECTED! Name: <color=blue>{col.gameObject.name}</color>");

            CanBePushed = false;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Obstacle"))
        {
            if (isDebugging)
                Debug.Log($"Obstacle LOST! Name: <color=blue>{col.gameObject.name}</color>");

            CanBePushed = true;
        }
    }
}
