namespace BattleDyzx
{
    [System.Serializable]
    public struct Vector
    {
        public static readonly Vector up = new Vector(0, 0, 1);

        public float x;
        public float y;
        public float z;

        public Vector( float x, float y, float z ) : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Normalize()
        {
            float sqrLen = x * x + y * y + z * z;

            if( sqrLen != 0.0f && sqrLen != 1.0f )
            {
                float invLength = FastMath.InvSqrt( sqrLen );
                x *= invLength;
                y *= invLength;
                z *= invLength;
            }
        }

        public static Vector operator *( Vector v, float f )
        {
            return new Vector( v.x * f, v.y * f, v.z * f );
        }

        public static Vector operator +( Vector v1, Vector v2 )
        {
            return new Vector( v1.x + v2.x, v1.y + v2.y, v1.z + v2.z );
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public float Dot(Vector v)
        {
            return x * v.x + y * v.y + z * v.z;
        }
    }
}