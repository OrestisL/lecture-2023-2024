using UnityEngine;

public class ManagedClass : MonoBehaviour
{
    public static ClassController controller;
    public virtual void OnEnable()
    { 
        if(controller == null) 
        {
            controller = ClassController.Instance;
        }
        controller.AddObject(this);
    }
    public virtual void OnUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnFixedUpdate() { }

    public virtual void OnDisable()
    {
        controller.RemoveObject(this);
    }
}
