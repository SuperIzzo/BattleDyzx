using System;
using UnityEngine;

using BattleDyzx;

//[ExecuteInEditMode]
//*
public class Arena : MonoBehaviour, IArena
{
    private IArenaReliefTopology heightTopology;
    private IArenaNormalTopology normalTopology;

    public Vector position
    {
        get { return new Vector(transform.position.x, transform.position.y, transform.position.z); }
    }

    public float SampleHeight( float x, float y, ArenaCoordType coordType )
    {
        Vector3 point = new Vector3( x, y );
        Vector3 localSpacePosition = transform.InverseTransformPoint( point );
        float height = heightTopology.SampleElevation( localSpacePosition.y + 0.5f, localSpacePosition.x + 0.5f, ArenaCoordType.ScaledOutput );
        return height + transform.position.y;
    }

    public Vector SampleNormal( float x, float y, ArenaCoordType coordType )
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        heightTopology = new BumpyGenArenaHeightTopology( 256, 256, 256, 8 );
        heightTopology = new ArenaHeightTopologyMemoizer( heightTopology );

        IArenaReliefTopology projHeightTopology = new TransformHeightTopologyFilter( heightTopology, Camera.main.projectionMatrix );
        projHeightTopology = new ArenaHeightTopologyMemoizer( projHeightTopology );

        normalTopology = new HeightBasedNormalTopology( projHeightTopology );
        
        Texture2D heightTexture = heightTopology.CreateHeightMapTexture();
        Texture2D normalTexture = normalTopology.CreateNormalMapTexture();

        var renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_MainTex", normalTexture);
    }
}
//*/