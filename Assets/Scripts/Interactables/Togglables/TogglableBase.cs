using UnityEngine;

public class TogglableBase : MonoBehaviour
{
    [SerializeField] protected float stateChangeDuration;

    public virtual void ChangeState()
    {
        Debug.Log($"Base method. Changing sate of <color=yellow>{gameObject.name}</color>");
    }
}
