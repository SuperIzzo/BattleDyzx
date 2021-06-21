namespace BattleDyzx
{
    public class BumpyGenArenaHeightTopology : IArenaReliefTopology
    {
        private struct Bump
        {
            public float x;
            public float y;
            public float size;
            public float elevation;
        }

        public float width { get; private set; }
        public float height { get; private set; }
        public float depth { get; private set; }
        public int numberOfBumps { get; private set; }
        private Bump[] bumps;

        public BumpyGenArenaHeightTopology( float width, float height, float depth, int numberOfBumps = 8 )
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.numberOfBumps = numberOfBumps;

            bumps = GenerateBumps( numberOfBumps );
        }

        private static Bump[] GenerateBumps( int numberOfBumps )
        {
            const float bumpSpread = 0.3f;
            const float bumpSize = 0.04f;
            const float bumpElevation = 0.2f;

            Bump[] bumps = new Bump[numberOfBumps];

            float bumpDist = bumpSpread;

            for( int i = 0; i < numberOfBumps; i++ )
            {
                float bx = Math.Cos( Math.PI * 2 * i / numberOfBumps );
                float by = Math.Sin( Math.PI * 2 * i / numberOfBumps );

                bumps[i].x = 0.5f + bx * bumpDist;
                bumps[i].y = 0.5f + by * bumpDist;
                bumps[i].size = bumpSize;
                bumps[i].elevation = bumpElevation;
            }

            return bumps;
        }

        public float SampleElevation( float x, float y )
        {
            x /= width;
            y /= height;

            if( x <= 0 || x >= 1 || y <= 0 || y >= 1 )
                return 0;

            float pDist = Math.Sqrt( Math.Pow( x - 0.5f, 2 ) + Math.Pow( y - 0.5f, 2 ) );
            float rate = pDist * 2.2f;

            rate = Math.Pow( rate, 3 );

            for( int i = 0; i < 8; i++ )
            {
                Bump b = bumps[i];
                float bumpHeight = Math.Sqrt( Math.Pow( x - b.x, 2 ) + Math.Pow( y - b.y, 2 ) );
                float elevation = 1 - Math.Pow( bumpHeight / b.size, 3 );

                if( elevation < 0 )
                    elevation = 0;

                rate += elevation * b.elevation;
            }

            rate = Math.Clamp01( rate );
            rate *= depth;

            return rate;
        }
    }
}