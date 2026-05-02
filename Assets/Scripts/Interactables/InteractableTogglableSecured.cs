using UnityEngine;

public class InteractableTogglableSecured : InteractableTogglable
{
    [SerializeField] private InteractableItem unlockingItem;

    override protected void Awake()
    {
        base.Awake();

        isInteractableOnce = true;
    }

    override protected void ChangeState()
    {
        if (unlockingItem != null && InteractionManager.Instance.ObjectInHands != null)
            if (unlockingItem.gameObject == InteractionManager.Instance.ObjectInHands)
            {
                IsInteractable = false;
                unlockingItem.Use();
            }
            else
            {
                hasBeenInteractedWith = false;
                return;
            }
        else
        {
            hasBeenInteractedWith = false;
            return;
        }

        base.ChangeState();
    }
}
