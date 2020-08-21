namespace BattleDyzx
{
    public class HeightBasedNormalTopology : IArenaNormalTopology
    {
        private IArenaReliefTopology heightTopology;

        public float depth { get { return heightTopology.depth; } }
        public float height { get { return heightTopology.height; } }
        public float width { get { return heightTopology.width; } }

        public HeightBasedNormalTopology( IArenaReliefTopology heightTopology )
        {
            this.heightTopology = heightTopology;
        }

        public Vector SampleNormal( float x, float y, ArenaCoordType coordType )
        {
            if( (coordType & ArenaCoordType.ScaledInput) == 0 )
            {
                x *= width;
                y *= height;
            }

            float heightX1 = heightTopology.SampleElevation( x - 1, y, ArenaCoordType.Scaled );
            float heightX2 = heightTopology.SampleElevation( x + 1, y, ArenaCoordType.Scaled );
            float heightY1 = heightTopology.SampleElevation( x, y - 1, ArenaCoordType.Scaled );
            float heightY2 = heightTopology.SampleElevation( x, y + 1, ArenaCoordType.Scaled );

            float dx = heightX1 - heightX2;
            float dy = heightY1 - heightY2;
            float dz = 2.0f;

            Vector normal = new Vector( dx, dy, dz);

            if( ( coordType & ArenaCoordType.ScaledOutput ) == 0 )
            {
                normal.Normalize();
            }

            return normal;
        }
    }
}