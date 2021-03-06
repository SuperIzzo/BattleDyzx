using System;
using UnityEngine;

using BattleDyzx;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class ArenaQuad : Arena
{
    void Start()
    {
        transform.localScale = new Vector3(
            arenaState.size,
            arenaState.size,
            1.0f);

        transform.position = new Vector3(
            arenaState.width / 2,
            0.0f,
            arenaState.height / 2);

        Texture2D heightTexture = arenaState.reliefTopology.CreateHeightMapTexture();
        Texture2D normalTexture = arenaState.normalTopology.CreateNormalMapTexture();

        var renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_BumpMap", normalTexture);
        renderer.material.SetTexture("_ParallaxMap", heightTexture);
    }
}