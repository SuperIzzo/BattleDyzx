using UnityEngine;
using BattleDyzx;

public class TransformHeightTopologyFilter : IArenaReliefTopology
{
    private IArenaReliefTopology topology;
    private Matrix4x4 transform;
    private Matrix4x4 inverseTransform;

    public float depth { get { return topology.depth; } }
    public float height { get { return topology.height; } }
    public float width { get { return topology.width; } }

    public TransformHeightTopologyFilter( IArenaReliefTopology topology,
                                          Matrix4x4 transform )
    {
        this.topology = topology;
        this.transform = transform;
        this.inverseTransform = transform.inverse;
    }

    public float SampleElevation( float x, float y )
    {
        Vector3 start = new Vector3( x - 0.5f, y - 0.5f, 1 );
        Vector3 end = new Vector3( x - 0.5f, y - 0.5f, 0 );

        Vector3 projStart = inverseTransform * start;
        Vector3 projEnd = inverseTransform * end;

        Vector3 projHit;
        float z = 0;
        if( Raycast( projStart, projEnd, out projHit ) )
        {
            Vector3 hit = transform * projHit;
            z = hit.z * depth;
        }

        return z;
    }

    private bool Raycast( Vector3 start, Vector3 end, out Vector3 hit )
    {
        if( Random.value < 0.01f )
            Debug.LogFormat( "Start:{0}  End:", start, end );

        Vector3 dir = end - start;
        float len = dir.magnitude;

        if( len > 0 )
        {
            dir = dir / len;
        }

        for( float i = 0; i <= len; i++ )
        {
            Vector3 sample = start + dir * i;
            float height = topology.SampleElevation( sample.x, sample.y );
            if( height > sample.z )
            {
                hit = sample;
                return true;
            }
        }

        hit = Vector3.zero;
        return false;
    }
}
