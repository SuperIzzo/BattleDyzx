using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BDXUnityStatics
{
    public static Vector3 ToUnityVector(this BattleDyzx.Vector bdxVector)
    {
        return new Vector3(bdxVector.x, bdxVector.z, bdxVector.y);
    }
}
