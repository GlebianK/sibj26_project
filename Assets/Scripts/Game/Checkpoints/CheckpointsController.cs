using UnityEngine;

public class CheckpointsController : MonoBehaviour
{
    public static CheckpointsController Instance { get; private set; }

    [SerializeField] private Checkpoint[] _checkpoints;

    public void ResetAllPoints()
    {
        foreach (var checkpoint in _checkpoints)
        {
            checkpoint.Visited = false;
        }

        Blackboard.LastCheckpointIndex.Value = 0;
        _checkpoints[0].Visited = true;
    }

    public Vector3 GetSpawnPosition()
    {
        var index = 0;

        if (Blackboard.LastCheckpointIndex.Value > 0
            || Blackboard.LastCheckpointIndex.Value < _checkpoints.Length)
        {
            index = Blackboard.LastCheckpointIndex.Value;
        }

        var point = _checkpoints[index];
        return point.SpawnPoint.position;
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning($"Instance of {GetType()} already created!");
        }

        SetupCheckpoints();
    }

    private void SetupCheckpoints()
    {
        for (int i = 0; i < _checkpoints.Length; i++)
        {
            _checkpoints[i].Setup(i);
        }
    }
}
