using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassController : GenericSingleton<ClassController>
{
    public List<ManagedClass> objects = new List<ManagedClass>();
    public override void Awake()
    {
        base.Awake();
        ManagedClass.controller = instance;
    }

    public void AddObject(ManagedClass mc)
    {
        if (!objects.Contains(mc))
            objects.Add(mc);
    }

    public void RemoveObject(ManagedClass mc)
    {
        if(objects.Contains(mc))
            objects.Remove(mc);
    }

    private void Update()
    {
        for(int i = 0; i < objects.Count; i++) 
        {
            objects[i].OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0;i < objects.Count; i++) 
        {
            objects[i].OnFixedUpdate();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].OnLateUpdate();
        }
    }
}
