using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTerrain : Arena
{
    public Texture2D terrainTexture;
    
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.heightmapPixelError = 0.2f;

        TerrainData terrainData = new TerrainData();

        float scale = arenaState.scale;
        terrainData.heightmapResolution = (int) arenaState.reliefTopology.width;
        terrainData.size = new Vector3(
            scale * arenaState.reliefTopology.width,
            scale * arenaState.reliefTopology.depth,
            scale * arenaState.reliefTopology.height);

        SplatPrototype splat = new SplatPrototype();
        splat.texture = terrainTexture;
        splat.tileSize = new Vector2(
            scale * arenaState.reliefTopology.width,
            scale * arenaState.reliefTopology.height);

        terrainData.splatPrototypes = new SplatPrototype[] { splat };

        float[,] heightData = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        int h = terrainData.heightmapResolution;
        int w = terrainData.heightmapResolution;

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                heightData[x, y] = arenaState.SampleElevation(x,y) / arenaState.reliefTopology.depth;
            }
        }

        terrainData.SetHeights(0, 0, heightData);

        terrain.terrainData = terrainData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
