using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float[] ToFloatArray(this Vector3 vector)
    {
        return new float[3] {vector.x, vector.y,vector.z};  
    }

    public static Vector3 ToVector3(this float[] floats)
    {
        if (floats.Length != 3)
            throw new System.Exception($"Array length {floats.Length} is not equal to 3");

        return new Vector3(floats[0], floats[1], floats[2]);

    }

    public static float[] ToFloatArray(this Quaternion quaternion)
    {
        return new float[4] { quaternion.x, quaternion.y, quaternion.z, quaternion.w };
    }

    public static Quaternion ToQuaternion(this float[] floats) 
    {
        if(floats.Length != 4)
            throw new System.Exception($"Array length {floats.Length} is not equal to 4");

        return new Quaternion(floats[0], floats[1], floats[2], floats[3]);
    }
}
