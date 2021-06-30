namespace BattleDyzx
{
    [System.Serializable]
    public struct Vector3D
    {
        public static readonly Vector3D up = new Vector3D(0, 0, 1);

        public float x;
        public float y;
        public float z;

        public Vector3D( float x, float y, float z ) : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3D operator *( Vector3D v, float f )
        {
            return new Vector3D( v.x * f, v.y * f, v.z * f );
        }
        public static Vector3D operator /(Vector3D v, float f)
        {
            return new Vector3D(v.x / f, v.y / f, v.z / f);
        }

        public static Vector3D operator +( Vector3D v1, Vector3D v2 )
        {
            return new Vector3D( v1.x + v2.x, v1.y + v2.y, v1.z + v2.z );
        }
        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public float Dot(Vector3D v)
        {
            return x * v.x + y * v.y + z * v.z;
        }

        public float Length()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public void Normalize()
        {
            float sqrLen = x * x + y * y + z * z;

            if (sqrLen != 0.0f && sqrLen != 1.0f)
            {
                float invLength = FastMath.InvSqrt(sqrLen);
                x *= invLength;
                y *= invLength;
                z *= invLength;
            }
        }
    }
}