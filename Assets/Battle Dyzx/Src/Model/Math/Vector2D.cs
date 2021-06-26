namespace BattleDyzx
{
    [System.Serializable]
    public struct Vector2D
    {
        public float x;
        public float y;

        public Vector2D(float x, float y) : this()
        {
            this.x = x;
            this.y = y;
        }

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

        public static Vector2D operator *(Vector2D v, float f)
        {
            return new Vector2D(v.x * f, v.y * f);
        }
        public static Vector2D operator /(Vector2D v, float f)
        {
            return new Vector2D(v.x / f, v.y / f);
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x - v2.x, v1.y - v2.y);
        }
    }
}