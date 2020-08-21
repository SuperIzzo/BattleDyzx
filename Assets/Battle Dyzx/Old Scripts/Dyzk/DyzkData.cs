using System;

[Serializable]
public struct DyzkData
{
    float   _weight;
    float   _radius;
    float   _spike;
    float   _balance;
    string  _image;
    
    // Primary stats
    public float  weight  { get { return _weight;  } }
    public float  radius  { get { return _radius;  } }
    public float  spike   { get { return _spike;   } }
    public float  balance { get { return _balance; } }
    public string image   { get { return _image;   } }

    // Secondary stats
    public float speed { get { return 1 / weight; } }

    public DyzkData( float weight, float radius, float spike, float balance, string image )
    {
        _weight     = weight;
        _radius     = radius;
        _spike      = spike;
        _balance    = balance;
        _image      = image;
    }
}
