using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool Visited { get; set; }

    [field: SerializeField] public Transform SpawnPoint { get; private set; }

    [SerializeField] private string _tag = "Player";

    private int _index = -1;

    public void Setup(int index)
    {
        _index = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Visited || _index < 0) return;

        var target = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject;

        if (target.CompareTag(_tag))
        {
            Visited = true;
            Blackboard.LastCheckpointIndex.Value = _index;
            Debug.Log($"Достигнута контрольная точка {_index}");
        }
    }
}
