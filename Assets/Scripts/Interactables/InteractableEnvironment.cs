using DG.Tweening;
using TMPro;
using UnityEditor.Compilation;
using UnityEngine;

public class InteractableEnvironment : InteractableBase
{
    [Header("References")]
    [SerializeField] GameObject platformMovable;
    [SerializeField] GameObject bottomPoint;
    [SerializeField] GameObject topPoint;

    [Space, Header("Values")]
    [SerializeField] private float moveDuration;
    [SerializeField] private bool isInitialBottom;

    private Vector3 targetPosition;

    public bool IsMoving { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (bottomPoint == null || topPoint == null)
        {
            Debug.LogError("Some of the elevator end-points are NULL!!!");
            return;
        }

        IsMoving = false;
        platformMovable.transform.position = isInitialBottom ? bottomPoint.transform.position : topPoint.transform.position;
        targetPosition = isInitialBottom ? topPoint.transform.position : bottomPoint.transform.position;
    }

    public override bool Interact()
    {
        base.Interact();
        if (isDebugging)
            Debug.Log("This is the <color=yellow>InteractableEnvironment's</color> Interact method ;)");

        if (IsMoving)
            return false;

        IsMoving = true;
        IsInteractable = false;

        platformMovable.transform.DOMoveY(targetPosition.y, moveDuration).OnComplete(() => ResetElevator()); 

        return true;
    }

    private void ResetElevator()
    {
        if (targetPosition == topPoint.transform.position)
            targetPosition = bottomPoint.transform.position;
        else
            targetPosition = topPoint.transform.position;
        
        IsMoving = false;
        IsInteractable = true;
        InteractionManager.Instance.CompleteInteraction();
    }
}

