using UnityEngine;

public struct DyzkAnalysis
{
    private static readonly byte alphaThreshold = 125;

    float   _maxRadius;
    float[] _allRadii;
    float   _angleSpan;
    float   _spike;
    Vector2 _centerOfMass;
    int     _numOpaquePixels;
    Vector2 _pivot;
    string  _imageURI;

    public Vector2  pivot           { get { return _pivot; } }
    public Vector2  centerOfMass    { get { return _centerOfMass;       } }
    public string   imageURI        { get { return _imageURI;          } }

    public float    maxRadius       { get{ return _maxRadius;           } }
    public int      numOpaquePixels { get { return _numOpaquePixels;    } }
    public float    weight          { get { return _numOpaquePixels;    } }    

    public float    spike
    {
        get
        {
            if( _spike >= 0 )
            {
                return _spike;
            }
            else
            {
                _spike = 0;

                foreach( float radius in _allRadii )
                {
                    float difference = maxRadius - radius;

                    // Tolerate a difference of up to 2 pixels
                    difference = Mathf.Max( difference - 2, 0 );

                    _spike += Mathf.Log( difference + 1 );
                }

                _spike /= _allRadii.Length;
                _spike /= Mathf.Log( 128 );

                return Mathf.Clamp01( _spike );
            }
        }
    }



    public float    balance
    {
        get
        {
            return (centerOfMass - pivot).magnitude / maxRadius;
        }
    }



    public DyzkData dyzkData
    {
        get
        {
            return new DyzkData( weight, maxRadius, spike, balance, imageURI );
        }
    }



    public void AnalyzeTexture( Texture2D tex, string imageURI = "" )
    {
        _imageURI = imageURI;
        _spike = -1;

        Color32[] pixels = tex.GetPixels32();
        _pivot = new Vector2( tex.width/2.0f, tex.height/2.0f );

        // Limit the angle loop to 1 pixel from a specific circle of precision
        // angSpan is an integer number that maps angles to a new range based on
        // the radius of precision, as interger 0..360 may leave a lot of holes
        float radiusOfPrecision = Mathf.Max( _pivot.x, _pivot.y );
        _angleSpan = Mathf.PI*2 / Mathf.Asin( 1/radiusOfPrecision );

        _allRadii = new float[ (int)_angleSpan ];

        _numOpaquePixels = 0;
        _maxRadius = 0;

        _centerOfMass = Vector2.zero;


        for( int i = 0; i < pixels.Length; i++ )
        {
            if( pixels[i].a > alphaThreshold )
            {
                Vector2 coords = new Vector2( i%tex.width, i/tex.width);
                _centerOfMass += coords;

                PolarVector2 polarCoords =
                    PolarVector2.FromCartesian( coords - _pivot );

                int angIdx = (int)( polarCoords.a/(Mathf.PI*2) * _angleSpan );

                if( polarCoords.r > _maxRadius )
                    _maxRadius = polarCoords.r;

                if( polarCoords.r > _allRadii[angIdx] )
                    _allRadii[angIdx] = polarCoords.r;

                _numOpaquePixels++;
            }
        }

        _centerOfMass /= _numOpaquePixels;        


        float balance = (_centerOfMass - _pivot).magnitude / _maxRadius;
        balance = Mathf.Clamp01( balance );
    }
}