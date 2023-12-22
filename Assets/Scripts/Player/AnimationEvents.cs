using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void ChangeColliderProperties()
    {
        if (SampleInputMapCreation.Instance.isCrouching)
        {
            SampleInputMapCreation.Instance.controller.height = 1.2f;
            SampleInputMapCreation.Instance.controller.center = new Vector3(0, 0.5f, 0.3f);
        }
        else
        {
            SampleInputMapCreation.Instance.controller.height = 1.6f;
            SampleInputMapCreation.Instance.controller.center = new Vector3(0, 0.8f, 0);
        }
    }
}
