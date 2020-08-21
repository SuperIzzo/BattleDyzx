using UnityEngine;
using System.Collections;

public class ArenaGen2 : MonoBehaviour
{
    struct Bump
    {
        public float x;
        public float y;
        public float size;
        public float elevation;
    }

    Bump[] bumps;
    int numBumps = 8;

    public Texture2D terrainTexture;

    // Use this for initialization
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();

        TerrainData terrainData = new TerrainData();

        terrainData.heightmapResolution = 512;
        terrainData.size = new Vector3( 256, 128, 256 );

        SplatPrototype splat = new SplatPrototype();
        splat.texture = terrainTexture;
        splat.tileSize = new Vector2( 256, 256 );
        terrainData.splatPrototypes = new SplatPrototype[] { splat };

        float[,] heightData = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        int h = terrainData.heightmapResolution;
        int w = terrainData.heightmapResolution;

        

        bumps = GenerateBumps( h, w, 8 );

        for( int x = 0; x < w; x++ )
        {
            for( int y = 0; y < h; y++ )
            {
                heightData[x, y] = GetHeightAtPoint( x, y, w, h );
            }
        }

        terrainData.SetHeights( 0, 0, heightData );

        terrain.terrainData = terrainData;
    }


    private static Bump[] GenerateBumps( int h, int w, int numBumps )
    {
        const float bumpSpread = 0.3f;
        const float bumpSize   = 0.04f;
        const float bumpElevation = 0.2f;

        Bump[] bumps = new Bump[numBumps];

        float bumpDist = w * bumpSpread;

        for( int i = 0; i < numBumps; i++ )
        {
            float bx = Mathf.Cos( Mathf.PI *2 *i/numBumps);
            float by = Mathf.Sin( Mathf.PI *2 *i/numBumps);

            bumps[i].x = w / 2 + bx * bumpDist;
            bumps[i].y = h / 2 + by * bumpDist;
            bumps[i].size = bumpSize * w;
            bumps[i].elevation = bumpElevation;
        }

        return bumps;
    }


    float GetHeightAtPoint( float x, float y, float w, float h )
    {
        if( x <= 0 || x >= w-1 || y <= 0 || y >= h-1 )
            return 0;

        float pDist = Mathf.Sqrt(Mathf.Pow(x-w/2, 2) + Mathf.Pow(y-h/2, 2));
        float rate = pDist*2.2f / h;
        
        rate = Mathf.Pow( rate, 3 );

        for( int i =0; i<8; i++)
        {
            Bump b = bumps[i];
            float bumpHeight = Mathf.Sqrt(Mathf.Pow(x-b.x, 2) + Mathf.Pow(y-b.y, 2));
            float elevation = 1 - Mathf.Pow(bumpHeight/b.size, 3);

            if( elevation < 0 )
                elevation = 0;

            rate += elevation * b.elevation;
        }

        rate = Mathf.Clamp01( rate );        

        return rate;
    }
}
