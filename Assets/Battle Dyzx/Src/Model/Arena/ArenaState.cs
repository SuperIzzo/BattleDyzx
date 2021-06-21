using System.Collections.Generic;

namespace BattleDyzx
{
    public class ArenaState
    {
        public IArenaReliefTopology reliefTopology;
        public IArenaNormalTopology normalTopology;

        public float SampleElevation(float x, float y)
        {
            return reliefTopology != null ? reliefTopology.SampleElevation(x, y) : 0.0f;
        }

        public Vector SampleNormal(float x, float y)
        {
            return normalTopology != null ? normalTopology.SampleNormal(x, y) : Vector.up;
        }
    }
}