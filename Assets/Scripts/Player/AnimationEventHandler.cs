using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public void EndClimb()
    {
        PlayerController.Instance.EndClimb();
    }
}
