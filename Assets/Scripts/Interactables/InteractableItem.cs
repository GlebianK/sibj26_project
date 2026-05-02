using System.Runtime.CompilerServices;
using UnityEngine;

public class InteractableItem : InteractableBase
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider itemCollider;
    [SerializeField] private float throwForce;

    public bool IsBeingCarried { get; private set; }

    override protected void Awake()
    {
        base.Awake();
        IsBeingCarried = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    override public bool Interact(GameObject newParent)
    {
        base.Interact(newParent);
        
        if (isDebugging)
            Debug.Log($"This is the <color=yellow>InteractableItem's</color> Interact method! GO: {gameObject.name}");

        IsBeingCarried = !IsBeingCarried;

        if (IsBeingCarried)
        {
            rb.useGravity = false;
            transform.SetParent(newParent.transform);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.SetParent(null);
            rb.useGravity = true;
            rb.AddRelativeForce(Vector3.forward * throwForce, ForceMode.VelocityChange);
        }

        return true;
    }
}
