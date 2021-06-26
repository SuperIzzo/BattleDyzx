using System.Collections.Generic;

namespace BattleDyzx
{
    public class BattleGameState
    {
        public List<DyzkState> dyzx = new List<DyzkState>();
        public ArenaState arena;
        public float dynamicsTimeStep;
        public Vector3D gravity;

        public DyzkState CreateDyzk(DyzkData dyzkData)
        {
            var state = new DyzkState();
            state.dyzkData = dyzkData;
            dyzx.Add(state);
            return state;
        }

        public ArenaState CreateArena(IArenaReliefTopology relief, IArenaNormalTopology normal, float size = 1.0f)
        {
            arena = new ArenaState();
            arena.reliefTopology = relief;
            arena.normalTopology = normal;
            arena.size = size;
            return arena;
        }
    }
}