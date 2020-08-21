namespace BattleDyzx
{
    public interface IArenaReliefTopology
    {
        float width { get; }
        float height { get; }
        float depth { get; }
        
        float SampleElevation( float x, float y, ArenaCoordType coordType = ArenaCoordType.Scaled );
    }
}