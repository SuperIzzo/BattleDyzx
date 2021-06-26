using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BDXUnityStatics
{
    public static Vector3 ToUnityVector(this BattleDyzx.Vector3D bdxVector)
    {
        return new Vector3(bdxVector.x, bdxVector.z, bdxVector.y);
    }

    public static Color ToUnityColor(this BattleDyzx.ColorRGBA bdxColor)
    {
        return new Color(bdxColor.r, bdxColor.g, bdxColor.b, bdxColor.a);
    }
    public static BattleDyzx.ColorRGBA ToBDXColorRBGA(this Color color)
    {
        return new BattleDyzx.ColorRGBA(color.r, color.g, color.b, color.a);
    }
}
