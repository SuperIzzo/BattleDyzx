namespace BattleDyzx
{
    /// <summary>
    /// A general relief (heightmap) topology interface.
    /// </summary>
    public interface IArenaReliefTopology
    {
        /// <summary>
        /// The width of the topology data.
        /// </summary>
        float width { get; }

        /// <summary>
        /// The height of the topology data.
        /// </summary>
        float height { get; }

        /// <summary>
        /// The depth of the topology data.
        /// </summary>
        float depth { get; }

        /// <summary>
        /// Samples the elevation at a specific point
        /// </summary>
        float SampleElevation( float x, float y );
    }
}