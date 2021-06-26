namespace BattleDyzx
{
    public struct PolarVector2D
    {
        public float a;
        public float r;

        public PolarVector2D(float angle, float radius)
        {
            a = angle;
            r = radius;
        }

        public Vector2D ToCartesian()
        {
            return new Vector2D(
                 Math.Cos(a) * r,        // x
                -Math.Sin(a) * r         // y
                );
        }

        public static PolarVector2D FromCartesian(float x, float y)
        {
            return new PolarVector2D(
                Math.Atan2(y, x),            // angle
                Math.Sqrt(x * x + y * y)     // radius
                );
        }

        public static PolarVector2D FromCartesian(Vector2D coords)
        {
            return FromCartesian(coords.x, coords.y);
        }

    }
}