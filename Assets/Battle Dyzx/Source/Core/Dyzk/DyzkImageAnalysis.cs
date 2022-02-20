namespace BattleDyzx
{
    public class DyzkImageAnalysis
    {
        const float EFFECTIVE_ALPHA_THRESHOLD = 0.5f;
        const float DYZK_DEPTH = 10.0f; // 10 unscaled pixels
        const float PIXEL_MASS_DENSITY = 300.0f;
        const float GRAVITY = 9.807f; // m/s2 - just used for "weight" stat

        IImageData _imageData;
        int _numOpaquePixels;
        Vector2D _centerOfMass;
        float _maxRadius;
        float[] _allRadii;
        float _size;
        float _saw;

        public IImageData imageData => _imageData;

        public float area => _numOpaquePixels * scale * scale;

        public Vector2D centerOfMass => _centerOfMass * scale;        

        public float maxRadius => _maxRadius * scale;

        public float scale => _size / Math.Max(_imageData.width, _imageData.height);

        public float size => _size;
        public float balance => 1.0f - (_centerOfMass - _imageData.GetSize()/2).length / _maxRadius;
        public float saw => _saw;
        public float volume => area * DYZK_DEPTH * scale;
        public float mass => volume * PIXEL_MASS_DENSITY;
        public float weight => mass * GRAVITY;

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

            float sawFactor = 0;
            for (int i = 0; i < allRadii.Length; i++)
            {
                const float PIXEL_CORRECTION = 2.0f; // raster circles can never be perfectly round (this helps us offset that)
                sawFactor += maxRadius - allRadii[i]- PIXEL_CORRECTION;
            }
            sawFactor = Math.Clamp01(sawFactor / allRadii.Length / maxRadius);
            sawFactor = Math.Sqrt(sawFactor);
            
            //Calculate center of mass
            centerOfMass /= numOpaquePixels;
            
            this._imageData = imageData;
            this._numOpaquePixels = numOpaquePixels;
            this._centerOfMass = centerOfMass;
            this._allRadii = allRadii;
            this._maxRadius = maxRadius;
            this._size = size;
            this._saw = sawFactor;
        }

        public DyzkData CreateDyzkData()
        {
            DyzkData dyzkData = new DyzkData();
            dyzkData.maxRadius = maxRadius;
            dyzkData.size = size;
            dyzkData.saw = saw;
            dyzkData.balance = balance;
            dyzkData.mass = mass;

            dyzkData.maxSpeed = 0.4f;
            dyzkData.maxRPM = 1000;            
            return dyzkData;
        }
    }

    static class DyzkImageAnalysisStatics
    {
    }
}
