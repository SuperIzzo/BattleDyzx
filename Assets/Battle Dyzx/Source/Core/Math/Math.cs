using System.Runtime.InteropServices;

namespace BattleDyzx
{
    [StructLayout( LayoutKind.Explicit )]
    public struct FloatIntUnion
    {
        [FieldOffset( 0 )]
        public float f;

        [FieldOffset( 0 )]
        public int i;
    }    

    public static class Math
    {
        public const float PI = (float)System.Math.PI;
        
        public static float Cos( float f )
        {
            return (float)System.Math.Cos(f);
        }        

        public static float Sin( float f )
        {
            return (float)System.Math.Sin(f);
        }
        public static float Acos(float f)
        {
            return (float)System.Math.Acos(f);
        }

        public static float Asin(float f)
        {
            return (float)System.Math.Asin(f);
        }

        public static float Pow( float f, float p )
        {
            return (float)System.Math.Pow( f, p );
        }

        public static float Sqrt( float f )
        {
            return (float)System.Math.Sqrt( f );
        }

        public static float Exp(float f)
        {
            return (float)System.Math.Exp(f);
        }

        public static float Atan(float d)
        {
            return (float)System.Math.Atan(d);
        }

        public static float Atan2(float y, float x)
        {
            return (float)System.Math.Atan2(y,x);
        }

        public static float Floor(float f)
        {
            return (float)System.Math.Floor(f);
        }

        public static float Ceiling(float f)
        {
            return (float)System.Math.Ceiling(f);
        }

        public static float Round(float f)
        {
            return (float)System.Math.Round(f);
        }

        public static float Round(float f, int decimals)
        {
            return (float)System.Math.Round(f, decimals);
        }

        public static float Clamp( float f, float min, float max )
        {
            return f <= min ? min
                 : f >= max ? max
                 : f;
        }

        public static int Clamp( int i, int min, int max )
        {
            return i <= min ? min
                 : i >= max ? max
                 : i;
        }

        public static float Clamp01( float f )
        {
            return Clamp(f, 0.0f, 1.0f);
        }

        public static float Wrap(float f, float min, float max)
        {
            float range = max - min;
            while (f < min)
            {
                f += range;
            }

            while (f > max)
            {
                f -= range;
            }

            return f;
        }

        public static float WrapAngle(float f)
        {
            return Wrap(f, 0, PI * 2);
        }

        public static float Min(float a, float b)
        {
            return a < b ? a : b;
        }
        public static int Min(int a, int b)
        {
            return a < b ? a : b;
        }

        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }

        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        public static float Abs(float f)
        {
            return f >= 0 ? f : -f;
        }
        public static int Abs(int i)
        {
            return i >= 0 ? i : -i;
        }

        public static float Sign(float f)
        {
            return f > 0 ? 1.0f :
                   f < 0 ? -1.0f :
                   0.0f;
        }

        public static int Sign(int i)
        {
            return i > 0 ? 1 :
                   i < 0 ? -1 :
                   0;
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