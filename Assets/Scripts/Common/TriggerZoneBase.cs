using UnityEngine;

public class TriggerZoneBase : MonoBehaviour
{
    public bool Triggered { get; set; }

    [SerializeField] private string _tag = "Player";
    [SerializeField] private bool _playOnce = true;
    [SerializeField] private GameObject[] _objectsToToggle;

    private void OnTriggerEnter(Collider other)
    {
        if (Triggered && _playOnce) return;

        var target = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject;

        if (target.CompareTag(_tag))
        {
            Triggered = true;

            foreach (var obj in _objectsToToggle)
            {
                obj.SetActive(!obj.activeInHierarchy);
            }
        }
    }
}
