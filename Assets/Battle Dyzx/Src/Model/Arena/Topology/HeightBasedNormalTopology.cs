namespace BattleDyzx
{
    public class ReliefBasedNormalTopology : IArenaNormalTopology
    {
        private IArenaReliefTopology heightTopology;

        public float depth { get { return heightTopology.depth; } }
        public float height { get { return heightTopology.height; } }
        public float width { get { return heightTopology.width; } }

        public ReliefBasedNormalTopology( IArenaReliefTopology heightTopology )
        {
            this.heightTopology = heightTopology;
        }

        public Vector3D SampleNormal( float x, float y )
        {
            float heightX1 = heightTopology.SampleElevation( x - 1, y );
            float heightX2 = heightTopology.SampleElevation( x + 1, y );
            float heightY1 = heightTopology.SampleElevation( x, y - 1 );
            float heightY2 = heightTopology.SampleElevation( x, y + 1 );

            float dx = heightX1 - heightX2;
            float dy = heightY1 - heightY2;
            float dz = 2.0f;

            Vector3D normal = new Vector3D( dx, dy, dz);
            normal.Normalize();

            return normal;
        }
    }
}