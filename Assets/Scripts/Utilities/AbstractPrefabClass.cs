using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPrefabClass : MonoBehaviour
{
    public virtual List<Material> Materials { get; set; } = new List<Material>();

    protected virtual void GetMaterials() 
    {
        Materials.Add(GetComponent<MeshRenderer>().material);

        for (int i = 0; i < transform.childCount; i++) 
        {
            Materials.Add(transform.GetChild(i).GetComponent<MeshRenderer>().material);
        }
    }
    public abstract void SetColor(Color color);
}
