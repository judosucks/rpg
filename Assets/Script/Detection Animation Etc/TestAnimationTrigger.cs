using UnityEngine;

public class TestAnimationTrigger : MonoBehaviour
{
    private void OnThunderStrike()
    {
        Debug.Log("Thunder strike");
        CameraManager.instance.newCamera.FollowThunder();
    }

    private void OnThunderComplete()
    {
        Debug.Log("Thunder complete");
        CameraManager.instance.newCamera.FollowPlayer();
    }
}
