using System.Collections.Generic;

namespace BattleDyzx
{
    public class ArenaState
    {
        public IArenaReliefTopology reliefTopology;
        public IArenaNormalTopology normalTopology;
        public float size { get; set; } = 1.0f;
        public float scale
        {
            get { return size / Math.Max(reliefTopology.width, reliefTopology.height); }
            set { size = value * Math.Max(reliefTopology.width, reliefTopology.height); }
        }
        public float width { get { return reliefTopology.width * scale; } }
        public float height { get { return reliefTopology.height * scale; } }
        public float depth { get { return reliefTopology.depth * scale; } }

        public float SampleElevation(float x, float y)
        {
            return reliefTopology != null ? reliefTopology.SampleElevation(x, y) : 0.0f;
        }

        public Vector3D SampleNormal(float x, float y)
        {
            return normalTopology != null ? normalTopology.SampleNormal(x, y) : Vector3D.forward;
        }

        public float SampleElevationScaled(float x, float y)
        {
            float s = scale;
            return SampleElevation(x / s, y / s) * s;
        }

        public Vector3D SampleNormalScaled(float x, float y)
        {
            float s = scale;
            return SampleNormal(x / s, y / s);
        }
    }
}