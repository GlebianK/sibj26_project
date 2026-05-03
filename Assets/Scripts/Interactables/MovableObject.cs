using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] private InteractableMovable imController;
    [SerializeField] private float disconnectAltitude;
    //[SerializeField] private Rigidbody rb;

    private void Update()
    {
        if (transform.parent != null)
        {
            if (Mathf.Abs(transform.position.y - transform.parent.position.y) > disconnectAltitude)
            {
                imController.Interact(null);
            }
            /*
            else if (rb.transform.localPosition.x < 0)
                rb.transform.localPosition = new Vector3(0f, rb.transform.localPosition.y, 0f);
            */
        }
    }
}
