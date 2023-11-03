using UnityEngine;

public class EarRotation : MonoBehaviour
{
    public float degreesPerFrame = 50.0f;
    void Update()
    {
        transform.rotation = Quaternion.AngleAxis(degreesPerFrame * Time.deltaTime, transform.right) * transform.rotation;
    }
}
