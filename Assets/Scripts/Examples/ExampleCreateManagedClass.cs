using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCreateManagedClass : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(CreateWithDelay(0.5f));
    }

    IEnumerator CreateWithDelay(float delay) 
    {
        while(Time.timeSinceLevelLoad <= 4f)
        {
            GameObject gameobject = new GameObject();
            gameobject.AddComponent<ManagedClass>();
            yield return new WaitForSeconds(delay);
        }
    }
}
