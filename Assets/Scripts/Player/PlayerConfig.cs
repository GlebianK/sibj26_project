using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; }

    [field: SerializeField, Header("Character controller settings")]
    public float Radius { get; private set; }
    [field: SerializeField] public float Height { get; private set; }
    [field: SerializeField] public Vector3 Center { get; private set; }

}
