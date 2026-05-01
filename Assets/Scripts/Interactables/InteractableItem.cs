using System.Runtime.CompilerServices;
using UnityEngine;

public class InteractableItem : InteractableBase
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider itemCollider;

    public bool IsBeingCarried { get; private set; }

    override protected void Awake()
    {
        base.Awake();
        IsBeingCarried = false;

        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    override public bool Interact(GameObject newParent)
    {
        base.Interact(newParent);
        
        if (isDebugging)
            Debug.Log($"This is the <color=yellow>InteractableItem's</color> Interact method! GO: {gameObject.name}");

        IsBeingCarried = !IsBeingCarried;

        if (IsBeingCarried)
        {
            transform.SetParent(newParent.transform);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.SetParent(null);
            rb.useGravity = true;
        }

        return true;
    }
}
