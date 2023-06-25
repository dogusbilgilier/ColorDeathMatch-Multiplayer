
using UnityEngine;


public static class MyUtils 
{
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void SetX(this Vector3 pos,float x)
    {
        pos = new Vector3(x, pos.y, pos.z);
    }
    public static void SetY(this Vector3 pos, float y)
    {
        pos = new Vector3(pos.x, y, pos.z);
    }
    public static void SetZ(this Vector3 pos, float z)
    {
        pos = new Vector3(pos.x, pos.y, z);
    }
}
