using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTerrain : Arena
{
    public Texture2D terrainTexture;
    
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();

        TerrainData terrainData = new TerrainData();

        terrainData.heightmapResolution = (int) arenaState.reliefTopology.width;
        terrainData.size = new Vector3(
            arenaState.reliefTopology.width,
            arenaState.reliefTopology.depth,
            arenaState.reliefTopology.height);

        SplatPrototype splat = new SplatPrototype();
        splat.texture = terrainTexture;
        splat.tileSize = new Vector2(
            arenaState.reliefTopology.width,
            arenaState.reliefTopology.height);

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
