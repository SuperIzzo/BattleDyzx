namespace BattleDyzx
{
    public interface IArenaNormalTopology
    {
        float width { get; }
        float height { get; }
        float depth { get; }
        
        Vector SampleNormal( float x, float y, ArenaCoordType coordType = ArenaCoordType.ScaledInput );
    }
}