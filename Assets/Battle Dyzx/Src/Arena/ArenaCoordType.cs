namespace BattleDyzx
{
    [System.Flags]
    public enum ArenaCoordType
    {
        Normalized = 0,
        ScaledInput = 1,
        ScaledOutput = 2,
        Scaled = ScaledInput | ScaledOutput
    }
}