using System;
using UnityEngine;

public class DialogueSequenceTrigger : MonoBehaviour
{
    [SerializeField] private DialogueSequenceConfig _config;

    private void OnEnable()
    {
        DialogueHandler.Instance.Play(_config);
        gameObject.SetActive(false);
    }
}
