namespace BattleDyzx
{
    public class GradientArenaReliefTopology : IArenaReliefTopology
    {
        private float _width;
        private float _height;
        private float _depth;

        public float width
        {
            get { return _width; }
            set
            {
                if( value == 0.0f ) { throw new IllegalDimensionsException(); }
                _width = value;
            }
        }

        public float height
        {
            get { return _height; }
            set
            {
                if( value == 0.0f ) { throw new IllegalDimensionsException(); }
                _height = value;
            }
        }

        public float depth
        {
            get { return _depth; }
            set
            {
                if( value == 0.0f ) { throw new IllegalDimensionsException(); }
                _depth = value;
            }
        }


        public float xGradient { get; set; }
        public float yGradient { get; set; }
        public float zBase { get; set; }

        public GradientArenaReliefTopology( float width, float height, float depth,
                                            float xGradient, float yGradient, float zBase )
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.xGradient = xGradient;
            this.yGradient = yGradient;
            this.zBase = zBase;
        }

        public float SampleElevation( float x, float y, ArenaCoordType coordType )
        {
            if( ( coordType & ArenaCoordType.ScaledInput ) != 0 )
            {
                x *= width;
                y *= height;
            }

            float z = zBase + x * xGradient + y * yGradient;

            if( ( coordType & ArenaCoordType.ScaledOutput ) == 0 )
            {
                z /= depth;
            }

            return z;
        }
    }
}