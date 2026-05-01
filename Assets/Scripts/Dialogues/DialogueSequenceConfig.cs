using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequenceConfig", menuName = "Scriptable Objects/DialogueSequenceConfig")]
public class DialogueSequenceConfig : ScriptableObject
{
    [field: SerializeField] public DialogueEvent[] Events { get; private set; }

    [Serializable]
    public class DialogueEvent
    {
        public string Text;
        public CharacterKey Actor;
    }
}
