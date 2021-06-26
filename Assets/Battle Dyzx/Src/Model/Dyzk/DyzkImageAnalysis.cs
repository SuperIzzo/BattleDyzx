namespace BattleDyzx
{
    public class DyzkImageAnalysis
    {
        const float EFFECTIVE_ALPHA_THRESHOLD = 0.5f;

        IImageData _imageData;
        int _numOpaquePixels;
        Vector2D _centerOfMass;
        float _maxRadius;
        float[] _allRadii;
        float _size;

        public IImageData imageData { get; private set; }

        public int numOpaquePixels { get; private set; }

        public Vector2D centerOfMass { get { return _centerOfMass * scale; } }

        public float maxRadius { get { return _maxRadius * scale; } }

        public float scale { get { return _size / Math.Max(_imageData.width, _imageData.height); } }

        public float size => _size;

        public DyzkImageAnalysis()
        {
        }

        public DyzkImageAnalysis(IImageData imageData, float size = 1)
        {
            AnalyzeImage(imageData, size);
        }

        public void AnalyzeImage(IImageData imageData, float size = 1)
        {
            float imageHalfWidth = imageData.width / 2;
            float imageHalfHeight = imageData.height / 2;

            // These are needed to collect discrete radius data (all round the circle) 
            float precisionRadius = Math.Max(imageHalfWidth, imageHalfHeight);
            float angleOfPrecision = Math.Asin(1.0f / precisionRadius);
            int numDiscreteAngles = (int)(Math.PI * 2 / angleOfPrecision);

            int numOpaquePixels = 0;
            float maxRadius = 0.0f;
            float[] allRadii = new float[numDiscreteAngles];
            var centerOfMass = new Vector2D();

            for (int x = 0; x<imageData.width; x++)
            {
                for (int y = 0; y<imageData.height; y++)
                {
                    ColorRGBA color = imageData.GetPixel(x, y);

                    // Skip transparent pixels
                    if (color.a < EFFECTIVE_ALPHA_THRESHOLD)
                    {                        
                        continue;
                    }

                    // Count opaque pixels
                    numOpaquePixels++;

                    // Accumulate center of mass coords
                    centerOfMass.x += x;
                    centerOfMass.y += y;

                    // Turn pixel coords into polar coords
                    var polarCoords = PolarVector2D.FromCartesian(x - imageHalfWidth, y - imageHalfHeight);
                    polarCoords.a = Math.WrapAngle(polarCoords.a);

                    // Update the max radius
                    maxRadius = Math.Max(maxRadius, polarCoords.r);

                    int angleIdx = (int)(polarCoords.a / angleOfPrecision);
                    allRadii[angleIdx] = Math.Max(allRadii[angleIdx], polarCoords.r);
                }
            }            
            
            //check we have at least 1 opaque pixel

            //Calculate center of mass
            centerOfMass /= numOpaquePixels;
            
            this._imageData = imageData;
            this._numOpaquePixels = numOpaquePixels;
            this._centerOfMass = centerOfMass;
            this._allRadii = allRadii;
            this._maxRadius = maxRadius;
            this._size = size;
        }

        public DyzkData CreateDyzkData()
        {
            DyzkData dyzkData = new DyzkData();
            dyzkData.maxRadius = maxRadius;
            dyzkData.size = size;
            dyzkData.maxSpeed = 0.4f;
            dyzkData.maxRPM = 1000;
            return dyzkData;
        }
    }

    static class DyzkImageAnalysisStatics
    {
    }
}
