namespace BattleDyzx
{
    [System.Serializable]
    public struct Vector2D
    {
        public float x;
        public float y;

        public float length => Math.Sqrt(x * x + y * y);

        public Vector2D normal
        {
            get
            {
                Vector2D normalVec = new Vector2D(x, y);
                normalVec.Normalize();
                return normalVec;
            }
        }

        public Vector2D(float x, float y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector3D(Vector2D v) => new Vector3D(v.x, v.y, 0.0f);
        public static Vector2D operator *(Vector2D v, float f) => new Vector2D(v.x * f, v.y * f);
        public static Vector2D operator /(Vector2D v, float f) => new Vector2D(v.x / f, v.y / f);
        public static Vector2D operator +(Vector2D v1, Vector2D v2) => new Vector2D(v1.x + v2.x, v1.y + v2.y);
        public static Vector2D operator -(Vector2D v1, Vector2D v2) => new Vector2D(v1.x - v2.x, v1.y - v2.y);
        public static Vector2D operator -(Vector2D v) => new Vector2D(-v.x, -v.y);
        public float Dot(Vector2D v) => x * v.x + y * v.y;
        public float Dot(Vector3D v) => x * v.x + y * v.y;

        public void Normalize()
        {
            float sqrLen = x * x + y * y;

            if (sqrLen != 0.0f && sqrLen != 1.0f)
            {
                float invLength = FastMath.InvSqrt(sqrLen);
                x *= invLength;
                y *= invLength;
            }
        }

        public void Normalize(out float length)
        {
            length = Math.Sqrt(x * x + y * y);
            if (length > 0)
            {
                x /= length;
                y /= length;
            }
        }

        public static readonly Vector2D zero = new Vector2D(0, 0);
        public static readonly Vector2D one = new Vector2D(1, 1);
        public static readonly Vector2D left = new Vector2D(-1, 0);
        public static readonly Vector2D right = new Vector2D(1, 0);
        public static readonly Vector2D up = new Vector2D(0, -1);
        public static readonly Vector2D down = new Vector2D(0, 1);
    }
}