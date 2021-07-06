using UnityEngine;
using System.Collections;

public class ArenaGen1 : MonoBehaviour
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



        bumps = GenerateBumps(numBumps);

        for( int x = 0; x < w; x++ )
        {
            for( int y = 0; y < h; y++ )
            {
                heightData[x, y] = GetHeightAtPoint( x / (float)w, y / (float) h );
            }
        }

        terrainData.SetHeights( 0, 0, heightData );

        terrain.terrainData = terrainData;
    }


    private static Bump[] GenerateBumps( int numBumps )
    {
        const float bumpSpread = 0.3f;
        const float bumpSize   = 0.04f;
        const float bumpElevation = 0.2f;

        Bump[] bumps = new Bump[numBumps];

        float bumpDist = bumpSpread;

        for( int i = 0; i < numBumps; i++ )
        {
            float bx = Mathf.Cos( Mathf.PI *2 *i/numBumps);
            float by = Mathf.Sin( Mathf.PI *2 *i/numBumps);

            bumps[i].x = 0.5f + bx * bumpDist;
            bumps[i].y = 0.5f + by * bumpDist;
            bumps[i].size = bumpSize;
            bumps[i].elevation = bumpElevation;
        }

        return bumps;
    }


    float GetHeightAtPoint( float x, float y )
    {
        if( x <= 0 || x >= 1 || y <= 0 || y >= 1 )
            return 0;

        float pDist = Mathf.Sqrt(Mathf.Pow(x-0.5f, 2) + Mathf.Pow(y-0.5f, 2));
        float rate = pDist*2.2f;

        rate = Mathf.Pow( rate, 3 );

        for( int i = 0; i < 8; i++ )
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
