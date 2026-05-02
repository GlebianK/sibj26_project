using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _stepSFX;

    public void EndClimb()
    {
        PlayerController.Instance.EndClimb();
    }

    public void Step()
    {
        _stepSFX.Play();
    }
}
