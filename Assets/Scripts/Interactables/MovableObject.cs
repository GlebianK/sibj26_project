using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] private InteractableMovable imController;
    [SerializeField] private float disconnectAltitude;

    private void Update()
    {
        if (transform.parent != null)
        {
            if (Mathf.Abs(transform.position.y - transform.parent.position.y) > disconnectAltitude)
            {
                imController.Interact(null);
            }
        }
    }
}
