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
    public virtual void OnUpdate() { Debug.Log(string.Format("{0} Update", GetType().Name)); }
    public virtual void OnLateUpdate() { Debug.Log(string.Format("{0} LateUpdate", GetType().Name)); }
    public virtual void OnFixedUpdate() { Debug.Log(string.Format("{0} FixedUpdate", GetType().Name)); }

    public virtual void OnDisable()
    {
        controller.RemoveObject(this);
    }
}
