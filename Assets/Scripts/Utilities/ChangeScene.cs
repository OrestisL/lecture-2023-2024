using Lecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public int index;

    private void OnTriggerEnter(Collider other)
    {
        if (index != 0)
            Util.ChangeScene(index);
    }
}

