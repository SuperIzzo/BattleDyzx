namespace BattleDyzx
{
    public interface IArenaNormalTopology
    {
        float width { get; }
        float height { get; }
        float depth { get; }
        
        Vector3D SampleNormal( float x, float y );
    }
}