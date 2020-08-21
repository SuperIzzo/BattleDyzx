using UnityEngine;


public struct PolarVector2
{
    public float a;
    public float r;    

    public PolarVector2(float angle, float radius)
    {
        a = angle;
        r = radius;
    }

    public Vector2 ToCartesian()
    {
        return new Vector2(
             Mathf.Cos( a ) * r,        // x
            -Mathf.Sin( a ) * r         // y
            );
    }

    public static PolarVector2 FromCartesian( float x, float y )
    {
        return new PolarVector2( 
            Mathf.Atan2( y, x ),            // angle
            Mathf.Sqrt( x * x + y * y )     // radius
            );
    }

    public static PolarVector2 FromCartesian( Vector2 coords )
    {
        return FromCartesian( coords.x, coords.y );
    }

}
