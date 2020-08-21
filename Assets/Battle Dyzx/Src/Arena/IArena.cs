namespace BattleDyzx
{
    public interface IArena
    {
        Vector position { get; }

        float SampleHeight( float x, float y, ArenaCoordType coordType );
        Vector SampleNormal( float x, float y, ArenaCoordType coordType );
    }
}