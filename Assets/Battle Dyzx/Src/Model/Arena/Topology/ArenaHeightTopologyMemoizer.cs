namespace BattleDyzx
{
    public class ArenaHeightTopologyMemoizer : IArenaReliefTopology
    {
        float[,] heightCache;
        IArenaReliefTopology heightTopology;

        public float depth { get { return heightTopology.depth; } }
        public float height { get { return heightTopology.height; } }
        public float width { get { return heightTopology.width; } }

        public float SampleElevation( float x, float y )
        {
            int xx = Math.Clamp( (int) x, 0, (int) width - 1 );
            int yy = Math.Clamp( (int) y, 0, (int) height - 1 );

            if( float.IsNaN( heightCache[xx, yy] ) )
            {
                heightCache[xx, yy] = heightTopology.SampleElevation( xx, yy );
            }
            float z = heightCache[xx, yy];

            return z;
        }

        public ArenaHeightTopologyMemoizer( IArenaReliefTopology heightTopology )
        {
            this.heightTopology = heightTopology;
            ResetBuffer();
        }

        private void ResetBuffer()
        {
            int width = (int) heightTopology.width;
            int height = (int) heightTopology.height;
            heightCache = new float[width, height];

            for( int y = 0; y < height; y++ )
            {
                for( int x = 0; x < width; x++ )
                {
                    heightCache[x, y] = float.NaN;
                }
            }
        }
    }
}