namespace BattleDyzx
{
    [System.Serializable]
    public struct Vector3D
    {
        public float x;
        public float y;
        public float z;

        public float length => Math.Sqrt(x * x + y * y + z * z);

        public Vector2D xy
        { 
            get { return new Vector2D(x, y); } 
            set { x = value.x; y = value.y; }
        }

        public Vector3D normal
        {
            get
            {
                Vector3D normalVec = new Vector3D(x, y, z);
                normalVec.Normalize();
                return normalVec;
            }
        }

        public Vector3D( float x, float y, float z ) : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3D operator *( Vector3D v, float f ) => new Vector3D( v.x * f, v.y * f, v.z * f );
        public static Vector3D operator /(Vector3D v, float f) => new Vector3D(v.x / f, v.y / f, v.z / f);
        public static Vector3D operator +( Vector3D v1, Vector3D v2 ) => new Vector3D( v1.x + v2.x, v1.y + v2.y, v1.z + v2.z );
        public static Vector3D operator -(Vector3D v1, Vector3D v2) => new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        public static Vector3D operator -(Vector3D v) => new Vector3D( -v.x, -v.y, -v.z);
        public float Dot(Vector3D v) => x * v.x + y * v.y + z * v.z;
        public float Dot(Vector2D v) => x * v.x + y * v.y;        

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

        public void Normalize(out float length)
        {
            length = Math.Sqrt(x * x + y * y + z * z);
            if (length > 0)
            {
                x /= length;
                y /= length;
                z /= length;
            }
        }

        public static readonly Vector3D zero = new Vector3D(0, 0, 0);
        public static readonly Vector3D one = new Vector3D(1, 1, 1);
        public static readonly Vector3D left = new Vector3D(-1, 0, 0);
        public static readonly Vector3D right = new Vector3D(1, 0, 0);
        public static readonly Vector3D up = new Vector3D(0, -1, 0);
        public static readonly Vector3D down = new Vector3D(0, 1, 0);
        public static readonly Vector3D back = new Vector3D(0, 0, -1);
        public static readonly Vector3D forward = new Vector3D(0, 0, 1);
    }
}