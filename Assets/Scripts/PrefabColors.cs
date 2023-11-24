using System;
using UnityEngine;

public class PrefabColors : AbstractPrefabClass
{
    [Flags]
    enum Options
    {
        one = 0,
        two = 2,
        three = 4,
        four = 8,
        five = 16,
        six = 32,
        seven = 64,
    }
    Options op = Options.one | Options.five | Options.six | Options.seven;
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

    void Switch()
    {
        switch (op)
        {
            case Options.one:
                break;
            case Options.two:
            case Options.three:
            case Options.four:
                //code
                break;
            case Options.five:
            case Options.six:
            case Options.seven:
                //some code
                break;
            default:
                return;
        }
    }
}
