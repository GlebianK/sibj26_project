using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _stepSFX;

    public void EndClimb()
    {
        PlayerController.Instance.EndClimb();
    }

    public void EndPunch()
    {
        PlayerController.Instance.EndPunch();
    }

    public void Step()
    {
        _stepSFX.Play();
    }
}
