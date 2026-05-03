using UnityEngine;

public class TogglableBase : MonoBehaviour
{
    [SerializeField] protected float stateChangeDuration;
    [SerializeField] protected GameObject controllableObject;

    public bool IsChangingState { get; protected set; }

    virtual public void ChangeState()
    {
        Debug.Log($"Base method. Changing sate of <color=yellow>{gameObject.name}</color>");
    }
}
