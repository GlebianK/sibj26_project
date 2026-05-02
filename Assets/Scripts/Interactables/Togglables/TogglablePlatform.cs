using DG.Tweening;
using UnityEngine;

public class TogglablePlatform : TogglableBase
{
    [SerializeField] private float moveHeight;
    [SerializeField] private bool isBottomDefault;

    private float moveDirection;

    private void Awake()
    {
        IsChangingState = false;

        if (isBottomDefault)
            moveDirection = 1;
        else
            moveDirection = -1;
    }

    public override void ChangeState()
    {
        if (IsChangingState)
            return;

        IsChangingState = true;
        controllableObject.transform.DOMoveY(controllableObject.transform.position.y + moveHeight * moveDirection, 
            stateChangeDuration).OnComplete(() => OnChangeStateCompleted());
    }

    private void OnChangeStateCompleted()
    {
        moveDirection *= -1;
        IsChangingState = false;
    }
}
