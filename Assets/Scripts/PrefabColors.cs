using UnityEngine;

public class PrefabColors : AbstractPrefabClass
{
    public override void SetColor(Color color)
    {
        GetMaterials();
        for (int i = 0; i < Materials.Count; i++) 
        {
            if (i % 2 == 0)
                Materials[i].color = color;
            else
                Materials[i].color = new Color(color.g, color.b, color.r);
        }
    }
}
