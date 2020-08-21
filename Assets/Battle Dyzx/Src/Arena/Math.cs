using System.Runtime.InteropServices;
using UnityEngine;

namespace BattleDyzx
{
    [StructLayout( LayoutKind.Explicit )]
    struct FloatIntUnion
    {
        [FieldOffset( 0 )]
        public float f;

        [FieldOffset( 0 )]
        public int i;
    }    

    public static class Math
    {
        public const float PI = Mathf.PI;

        public static float Cos( float f )
        {
            return Mathf.Cos(f);
        }

        public static float Sin( float f )
        {
            return Mathf.Sin( f );
        }

        public static float Pow( float f, float p )
        {
            return Mathf.Pow( f, p );
        }

        public static float Sqrt( float f )
        {
            return Mathf.Sqrt( f );
        }

        public static float Clamp( float f, float min, float max )
        {
            return Mathf.Clamp( f, min, max );
        }

        public static int Clamp( int f, int min, int max )
        {
            return Mathf.Clamp( f, min, max );
        }

        public static float Clamp01( float f )
        {
            return Mathf.Clamp01( f );
        }
    }

    public static class FastMath
    {
        public static float InvSqrt( float f )
        {            
            FloatIntUnion fi;
            fi.i = 0;
            fi.f = f;
            fi.i = 0x5f3759df - ( fi.i >> 1 );
            return fi.f * ( 1.5f - 0.5f * f * fi.f * fi.f );
        }
    }
}